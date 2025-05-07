using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGL.Auth.Domain.Entities
{
    public class FeatureAccess
    {
        public int AccessID { get; set; }
       // public int PortalID { get; set; }
        public string AccessName { get; set; }
        public int RoleAccessMappingID { get; set; }
        public bool IsActive { get; set; }
    }

}
