using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGL.Auth.Application.DTOs.Account
{
    public class UserDTO
    {
        public int? UserID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CompanyName { get; set; }
        public string? MobileNumber { get; set; }
        public string? EmailID { get; set; }
        public bool? IsActive { get; set; }
        public int? StatusID { get; set; }
    }
}
