using System;
using System.Collections.Generic;
using System.Text;

namespace TMS.Common
{
   public class Enums
    {
      public  enum StatusCode
        {
          Success=1,
          Failed,
          Exception,
          UnAuthorized
        }
        public enum Status
        {
            Active = 1,
            InActive,
            Deleted
        }
        public enum UserStatus
        {
            Active = 1,
            InActive,
            Deleted,
            Blocked
        }
    }
}
