using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGL.Auth.Domain.Entities
{
    public class UserDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
    }

    public class UpdateUserStatusById
    {
        public int UserId { get; set; }
    }
    public class GetUpdatedUserDetails
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsVerified { get; set; }
        public bool IsMandateDone { get; set; }
        public int PortalID { get; set; }
        public string PortalName { get; set; }
        public string PortalKey { get; set; }
        public string Status { get; set; }

    }
}
