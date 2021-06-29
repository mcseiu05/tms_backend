using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TMS.BLL;
using TMS.Common.Model;
using TMS.Common.ViewModel;
using TMS.DAL.Model;

namespace TMS.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
    
        private readonly IUserManager userManager;
        private int userId;
        public UsersController(IUserManager _userManager)
        {
            
            this.userManager = _userManager;
           
        }

        // GET: api/Users
        [HttpGet("all")]
        public async Task<ResponseMessage> GetUsers()
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                await userManager.GetAllUser(response);
            }
            catch (Exception ex)
            {
                response.Message = "An Error has occured";
                response.StatusCode = (int)Common.Enums.StatusCode.Exception;
            }
            return response;
        }
       
        [HttpPost("register")]
        public async Task<ResponseMessage> RegisterUsers(RequestMessage message)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                this.GetUserIdFromToken();
                Users users = JsonConvert.DeserializeObject<Users>(message.RequestObj.ToString());
                await userManager.InsertUser(users,response,userId);
            }
            catch (Exception)
            {
                response.StatusCode = (int)Common.Enums.StatusCode.Exception;
                response.Message = "An error has occurred";
            }
           


            return response ;
        }

        [HttpPost("update")]
        public async Task<ResponseMessage> UpdateUsers(RequestMessage message)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                this.GetUserIdFromToken();
                Users users = JsonConvert.DeserializeObject<Users>(message.RequestObj.ToString());
                await userManager.UpdateUser(users,response,userId);
            }
            catch (Exception)
            {
                response.StatusCode = (int)Common.Enums.StatusCode.Exception;
                response.Message = "An error has occurred";
            }

            return response;
        }

        [HttpPost("delete")]
        public async Task<ResponseMessage> DeleteUsers(RequestMessage message)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                this.GetUserIdFromToken();
                Users users = JsonConvert.DeserializeObject<Users>(message.RequestObj.ToString());
                await userManager.DeleteUser(users, response, userId);

            }
            catch (Exception)
            {
                response.StatusCode = (int)Common.Enums.StatusCode.Exception;
                response.Message = "An error has occurred";
            }



            return response;
        }
      
        [HttpPost("id")]
        public async Task<ResponseMessage> GetUserById(RequestMessage message)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                int userId = JsonConvert.DeserializeObject<int>(message.RequestObj.ToString());
                await userManager.GetUserById(userId, response);
            }
            catch (Exception)
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
    }
}
