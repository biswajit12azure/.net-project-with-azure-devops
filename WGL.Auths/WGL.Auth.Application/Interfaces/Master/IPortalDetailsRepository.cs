using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGL.Auth.Application.DTOs.Master;

namespace WGL.Auth.Application.Interfaces.Master
{
    public interface IPortalDetailsRepository
    {
        Task<List<PortalDTO>> GetPortalDetails();
    }
}
