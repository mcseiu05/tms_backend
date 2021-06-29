using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMS.Common.Model;
using TMS.DAL.Model;

namespace TMS.BLL
{
   public class AdminManager
    {
        private TMSContext context;

        private AdminManager(TMS.DAL.Model.TMSContext mSContext)
        {
            this.context = mSContext;
        }

        #region users

        public async Task<int> InsertUser(Users users)
        {
            context.Users.Add(users);
            return await context.SaveChangesAsync();
        }

        #endregion
    }
}
