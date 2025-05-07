using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGL.Auth.Domain.Entities
{
    public class PortalRoleAccessResponse
    {
        public int PortalID { get; set; }
        //public int RoleID { get; set; }
        public string PortalName { get; set; }
        public UserDetails UserDetails { get; set; }
        public List<PortalRole> PortalRoleAccess { get; set; }
    }

}
