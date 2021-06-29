using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Common.Model;
using TMS.Common.ViewModel;
using TMS.DAL.Model;

namespace TMS.BLL
{
    public class TasksManager : ITasksManager
    {
        private TMSContext context;

        public TasksManager(DAL.Model.TMSContext mSContext)
        {
            this.context = mSContext;
        }
        public Task DeleteTasks(RequestMessage message, ResponseMessage response, int userID)
        {
            throw new NotImplementedException();
        }

        public async Task GetAllTasks(ResponseMessage response, int userId)
        {
            response.ResponseObj = await context.Task
                .Join(context.Users, x => x.AssignedBy, y => y.Id, (t, u) =>
                 new
                 {
                     t,
                     u
                 }).Where(x => x.t.AssignedTo == userId).ToListAsync();
        }

        public async Task GetTasksById(int taskId, ResponseMessage response)
        {
            response.ResponseObj = await context.Task.FindAsync(taskId);
            if (response.ResponseObj == null)
            {
                response.Message = "Invalid task id";
            }
        }

        public async Task InsertTasks(Tasks task, ResponseMessage response, int userID)
        {

            task.AssignedDate = DateTime.Now;
            task.AssignedBy = userID;
            context.Task.Add(task);
            if (await context.SaveChangesAsync() > 0)
            {
                response.Message = "Task has been assigned successfully.";
            }
            else
            {
                response.Message = "Tasks saved failed.";
            }

        }

        public async Task UpdateTasks(Tasks tasks, ResponseMessage response, int userID)
        {

            tasks.AssignedDate = DateTime.Now;
            tasks.AssignedBy = userID;
            if (!TasksExists(tasks.Id))
            {
                response.Message = "Invalid Object";
                response.StatusCode = (int)Common.Enums.StatusCode.Failed;
                return;
            }
            context.Entry(tasks).State = EntityState.Modified;
            if (await context.SaveChangesAsync() > 0)
            {
                response.Message = "Task has been updated successfully.";
            }
            else
            {
                response.Message = "Tasks update failed.";
            }
        }
        private bool TasksExists(int id)
        {
            return context.Task.Any(e => e.Id == id);
        }
    }
}
