using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGL.Auth.Application.CQRS.Account.Queries.BusinessCategory;
using WGL.Auth.Application.Interfaces.Account;
using WGL.Auth.Domain.Entities;
using WGL.Utility.Wrappers;

namespace WGL.Auth.Application.CQRS.Account.Queries.Classification
{
    public class GetClassificationQuery: IRequest<Response<List<ClassificationResponse>>>
    {
    }
    public class GetClassificationQueryHandler : IRequestHandler<GetClassificationQuery, Response<List<ClassificationResponse>>>
    {
        private readonly IAccountRepositoryAsync _classification;
        private readonly IMapper _mapper;
        public GetClassificationQueryHandler(IAccountRepositoryAsync classification, IMapper mapper)
        {
            _classification = classification;
            _mapper = mapper;
        }
        public async Task<Response<List<ClassificationResponse>>> Handle(GetClassificationQuery request, CancellationToken cancellationToken)
        {
            var result = await _classification.GetClassification();
            return new Response<List<ClassificationResponse>>(result, message: "Classification Received Successfully!!");
        }
    }
}
