using System;
using System.Collections.Generic;

namespace TMS.Common.Model
{
    public partial class Users
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public int? Status { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public int? RegisteredBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
