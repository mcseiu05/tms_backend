using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMS.Common.Model;
using TMS.Common.ViewModel;

namespace TMS.BLL
{
   public interface IUserManager
    {
         Task InsertUser(Users users,ResponseMessage response,int userID);
        Task UpdateUser(Users users, ResponseMessage response, int userID);
        Task DeleteUser(Users users, ResponseMessage response, int userID);
        Task GetAllUser(ResponseMessage response);
        Task GetUserById(int userId, ResponseMessage response);
        Task<Users> GetUserByEmailAndPassword(string email, string password);
    }
}
