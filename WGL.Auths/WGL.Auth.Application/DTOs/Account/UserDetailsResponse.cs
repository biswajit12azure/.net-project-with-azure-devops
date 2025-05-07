using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGL.Auth.Application.DTOs.Account
{
    public class UserDetailsResponse
    {
        public int? UserID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailID { get; set; }
        public bool? IsVerified { get; set; }
        public bool? IsMandateDone { get; set; }
        public string? Status { get; set; }
        public int? AgencyID { get; set; }
        public int? RoleID { get; set; }
    }
}
