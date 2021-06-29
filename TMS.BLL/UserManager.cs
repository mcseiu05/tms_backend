using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Common.Model;
using TMS.Common.ViewModel;
using TMS.DAL.Model;

namespace TMS.BLL
{
    public class UserManager : IUserManager
    {
        private TMSContext context;

        public UserManager(DAL.Model.TMSContext mSContext)
        {
            this.context = mSContext;
        }
        public async Task DeleteUser(Users users, ResponseMessage response, int userID)
        {
            if (!UsersExists(users.Id))
            {
                response.Message = "Invalid user";
                response.StatusCode = (int)Common.Enums.StatusCode.Failed;
                return;
            }
            users.UpdatedBy = userID;
            users.UpdatedDate = DateTime.Now;
            users.Status = (int)Common.Enums.UserStatus.Deleted;
            context.Entry(users).State = EntityState.Modified;
            if (await context.SaveChangesAsync() > 0)
            {
                response.Message = "User deleted successfully.";
            }
            else
            {
                response.Message = "User delete failed";
            }
        }

        public async Task GetAllUser(ResponseMessage response)
        {
            response.ResponseObj =await context.Users.Where(x => x.Status == (int)Common.Enums.UserStatus.Active).ToListAsync();
        }

        public async Task<Users> GetUserByEmailAndPassword(string email, string password)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }

        public async Task GetUserById(int userId, ResponseMessage response)
        {
            response.ResponseObj = await context.Users.FindAsync(userId);
            if (response.ResponseObj == null)
            {
                response.Message = "Invalid user id";
            }
        }

        public async Task InsertUser(Users users, ResponseMessage response, int userID)
        {
            //TO DO:password should be encrypted

            users.RegisteredBy = userID;
            users.RegistrationDate = DateTime.Now;
            users.Status = (int)Common.Enums.UserStatus.Active;
            context.Users.Add(users);
            if (await context.SaveChangesAsync() > 0)
            {
                response.Message = "User registration successfull.";
            }
            else
            {
                response.Message = "User registration failed";
            }
        }

        public async Task UpdateUser(Users users, ResponseMessage response, int userID)
        {
            //TO DO:password should be encrypted

            if (!UsersExists(users.Id))
            {
                response.Message = "Invalid user";
                response.StatusCode = (int)Common.Enums.StatusCode.Failed;
                return;
            }

            users.UpdatedBy = userID;
            users.UpdatedDate = DateTime.Now;

            context.Entry(users).State = EntityState.Modified;
            if (await context.SaveChangesAsync() > 0)
            {
                response.Message = "User update successfull.";
            }
            else
            {
                response.Message = "User update failed";
            }

        }
        private bool UsersExists(int id)
        {
            return context.Users.Any(e => e.Id == id);
        }
    }
}
