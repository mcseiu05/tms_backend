using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class TasksController : ControllerBase
    {
        private readonly TMSContext _context;
        private int userId;
        private ITasksManager taskManager;

        public TasksController(ITasksManager _tasksManager)
        {
            this.taskManager = _tasksManager;
        }

        // GET: api/Tasks
        [HttpGet("all")]
        public async Task<ResponseMessage> GetTasks()
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                GetUserIdFromToken();
                await taskManager.GetAllTasks(response,userId);
            }
            catch (Exception ex)
            {
                response.Message = "An Error has occured";
                response.StatusCode = (int)Common.Enums.StatusCode.Exception;
            }
            return response;
        }

        [HttpPost("save")]

        public async Task<ResponseMessage> SaveTask(RequestMessage message)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                GetUserIdFromToken();
                dynamic task = JsonConvert.DeserializeObject(message.RequestObj.ToString());
                task.StartDate = DateTime.ParseExact(task.StartDate.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                task.EndDate = DateTime.ParseExact(task.EndDate.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                Tasks tasks = JsonConvert.DeserializeObject<Tasks>(JsonConvert.SerializeObject(task));
                await taskManager.InsertTasks(tasks, response, userId);
            }
            catch (Exception)
            {
                response.StatusCode = (int)Common.Enums.StatusCode.Exception;
                response.Message = "An error has occurred";
            }



            return response;
        }
        [HttpPost("update")]

        public async Task<ResponseMessage> UpdateTask(RequestMessage message)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                GetUserIdFromToken();
                dynamic tasks = JsonConvert.DeserializeObject(message.RequestObj.ToString());

                tasks.StartDate = DateTime.ParseExact(tasks.StartDate.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                tasks.EndDate = DateTime.ParseExact(tasks.EndDate.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                Tasks tasksObj = JsonConvert.DeserializeObject<Tasks>(JsonConvert.SerializeObject(tasks));
                await taskManager.UpdateTasks(tasksObj, response, userId);
            }
            catch (Exception)
            {
                response.StatusCode = (int)Common.Enums.StatusCode.Exception;
                response.Message = "An error has occurred";
            }



            return response;
        }
        [HttpPost("delete")]

        public async Task<ResponseMessage> DeleteTask(RequestMessage message)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                GetUserIdFromToken();
                await taskManager.DeleteTasks(message, response, userId);
            }
            catch (Exception)
            {
                response.StatusCode = (int)Common.Enums.StatusCode.Exception;
                response.Message = "An error has occurred";
            }
            return response;
        }
        [HttpPost("id")]
        public async Task<ResponseMessage> GetTasksById(RequestMessage message)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                int taskId = JsonConvert.DeserializeObject<int>(message.RequestObj.ToString());
                await taskManager.GetTasksById(taskId, response);
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
