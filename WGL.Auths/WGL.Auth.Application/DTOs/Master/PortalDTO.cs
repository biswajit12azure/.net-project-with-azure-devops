using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGL.Auth.Application.DTOs.Master
{
    public class PortalDTO
    {
        public int PortalID { get; set; }
        public string? PortalName { get; set; }
        public string? PortalDescription { get; set; }  
        public string? PortalKey { get; set; }
    }
}
