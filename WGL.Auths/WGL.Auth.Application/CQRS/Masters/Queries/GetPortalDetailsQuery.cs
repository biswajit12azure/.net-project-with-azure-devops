using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WGL.Auth.Application.DTOs.Master;
using WGL.Auth.Application.Interfaces.Master;
using WGL.Auth.Domain.Entities;
using WGL.Utility.Wrappers;

namespace WGL.Auth.Application.CQRS.Masters.Queries
{
    public class GetPortalDetailsQuery : IRequest<Response<List<PortalDTO>>>
    {

    }
    public class GetPortalDetailsQueryHandler : IRequestHandler<GetPortalDetailsQuery, Response<List<PortalDTO>>>
    {
        private readonly IPortalDetailsRepository _portalDetailsRepository;
        private readonly IMapper _mapper;
        public GetPortalDetailsQueryHandler(IPortalDetailsRepository portalDetailsRepository, IMapper mapper)
        {
            _portalDetailsRepository = portalDetailsRepository;
            _mapper = mapper;
        }
        public async Task<Response<List<PortalDTO>>> Handle(GetPortalDetailsQuery request, CancellationToken cancellationToken)
        {

            var result = await _portalDetailsRepository.GetPortalDetails();
            return new Response<List<PortalDTO>>(result, message: "Dcoument Type Received Successfully!!");
        }
    }
}
