using System;
using System.Collections.Generic;
using System.Text;

namespace TMS.Common.ViewModel
{
   public class ResponseMessage
    {
        public object ResponseObj { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public int StatusCode { get; set; } = (int)Common.Enums.StatusCode.Success;
    }
}
