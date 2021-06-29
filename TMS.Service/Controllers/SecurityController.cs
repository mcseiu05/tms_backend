
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TMS.BLL;
using TMS.Common.Model;
using TMS.Common.ViewModel;

namespace TMS.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        public IConfiguration _configuration;
        private IUserManager userManager;
        private int userId;

        public SecurityController(IUserManager _userManager,IConfiguration configuration)
        {
            this.userManager = _userManager;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<ResponseMessage> Login(TMS.Common.ViewModel.RequestMessage message)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                Users _userData = JsonConvert.DeserializeObject<Users>(message.RequestObj.ToString());
                if (_userData != null && _userData.Email != null && _userData.Password != null)
                {
                    var user = await userManager.GetUserByEmailAndPassword(_userData.Email, _userData.Password);

                    if (user != null)
                    {
                        //create claims details based on the user information
                        GenerateToken(user, response);
                    }
                    else
                    {
                        response.Message = "Invalid credentials";
                        response.StatusCode = (int)Common.Enums.StatusCode.Failed;
                    }
                }
                else
                {
                    response.Message = "Invalid request";
                    response.StatusCode = (int)Common.Enums.StatusCode.Failed;
                }
            }
            catch (Exception ex)
            {
                response.Message = "An Error has occurred";
                response.StatusCode = (int)Common.Enums.StatusCode.Failed;
            }
           

            return response;
        }

        [HttpGet("logout")]
        public async Task<ResponseMessage> Logout()
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                GetUserIdFromToken();
                //TO DO: Using userId logout functionality;
                response.Message = "Logout successfully";
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)Common.Enums.StatusCode.Exception;
                response.Message = "An error has occurred";
            }
            return response;
        }
        private void GetUserIdFromToken()
        {
            string tokenStr = Request.Headers[TMS.Common.Constants.Authorization].ToString();
            var token = new JwtSecurityTokenHandler().ReadJwtToken(tokenStr).Payload;
            this.userId = Convert.ToInt32(token["Id"].ToString());
        }
        private void  GenerateToken(Users user,ResponseMessage response)
        {
            var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("FirstName", user.FirstName),
                    new Claim("LastName", user.LastName),
                    new Claim("Email", user.Email)
                   };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
            response.Token = new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}