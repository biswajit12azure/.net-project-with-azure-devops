using AutoMapper;
using MediatR;
using Microsoft.Exchange.WebServices.Data;
using Serilog.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGL.Auth.Application.CQRS.Account.Queries.Mapping;
using WGL.Auth.Application.DTOs.Account;
using WGL.Auth.Application.Interfaces.Account;
using WGL.Auth.Application.Interfaces.UserPortalRoleMapping;
using WGL.Auth.Domain.Entities;
using WGL.Utility.Wrappers;

namespace WGL.Auth.Application.CQRS.Account.Queries.DocumentType
{
    public class GetDocumentTypeQuery : IRequest<Response<List<DocumentTypeResponse>>>
    {
        public string PortalKey { get; set; }
        public GetDocumentTypeQuery(string portalKey)
        {
            PortalKey = portalKey;
        }
    }
    public class GetDocumentTypeQueryHandler : IRequestHandler<GetDocumentTypeQuery, Response<List<DocumentTypeResponse>>>
    {
        private readonly IAccountRepositoryAsync _documnetType;
        private readonly IMapper _mapper;
        public GetDocumentTypeQueryHandler(IAccountRepositoryAsync documentType, IMapper mapper)
        {
            _documnetType = documentType;
            _mapper = mapper;
        }
        public async Task<Response<List<DocumentTypeResponse>>> Handle(GetDocumentTypeQuery request, CancellationToken cancellationToken)
        {
            var result = await _documnetType.GetDocumentType(request.PortalKey);
            return new Response<List<DocumentTypeResponse>>(result, message: "Dcoument Type Received Successfully!!");
        }
    }
}
