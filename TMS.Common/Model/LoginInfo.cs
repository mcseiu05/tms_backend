using System;
using System.Collections.Generic;

namespace TMS.Common.Model
{
    public partial class LoginInfo
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public DateTime? LoginTime { get; set; }
        public DateTime? LoggedOutTime { get; set; }
        public int? LoggedinStatus { get; set; }
    }
}
