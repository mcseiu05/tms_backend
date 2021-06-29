using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMS.Common.Model;
using TMS.Common.ViewModel;

namespace TMS.BLL
{
   public interface ITasksManager
    {
        Task InsertTasks(Tasks tasks, ResponseMessage response, int userID);
        Task UpdateTasks(Tasks task, ResponseMessage response, int userID);
        Task DeleteTasks(RequestMessage message, ResponseMessage response, int userID);
        Task GetAllTasks(ResponseMessage response,int userId);
        Task GetTasksById(int taskId, ResponseMessage response);
    }
}
