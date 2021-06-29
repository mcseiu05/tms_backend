using System;
using System.Collections.Generic;
using System.Text;

namespace TMS.Common.ViewModel
{
   public class Payload
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public DateTime IssuedTime { get; set; }
        public DateTime ExpiredTime { get; set; } 


    }
}
