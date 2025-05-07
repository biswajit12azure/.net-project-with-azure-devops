using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGL.Auth.Domain.Entities
{
    public class UserPortalRoleMappingResult
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailID { get; set; }
        public int PortalID { get; set; }
        public string PortalName { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string AccessName { get; set; }
        public int AccessID { get; set; }
        public bool IsAccessActive { get; set; }

    }

}
