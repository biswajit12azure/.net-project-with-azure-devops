using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGL.Auth.Domain.Entities
{
    public class PortalRole
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public List<FeatureAccess> FeatureAccess { get; set; }=new List<FeatureAccess>();
    }

}
