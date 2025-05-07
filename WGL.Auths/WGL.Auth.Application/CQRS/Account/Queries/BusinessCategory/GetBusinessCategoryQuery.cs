using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGL.Auth.Application.CQRS.Account.Queries.DocumentType;
using WGL.Auth.Application.Interfaces.Account;
using WGL.Auth.Domain.Entities;
using WGL.Utility.Wrappers;

namespace WGL.Auth.Application.CQRS.Account.Queries.BusinessCategory
{
    public class GetBusinessCategoryQuery: IRequest<Response<List<BusinessCategoryResponse>>>
    {
    }
    public class GetBusinessCategoryQueryHandler : IRequestHandler<GetBusinessCategoryQuery, Response<List<BusinessCategoryResponse>>>
    {
        private readonly IAccountRepositoryAsync _businessCategory;
        private readonly IMapper _mapper;
        public GetBusinessCategoryQueryHandler(IAccountRepositoryAsync businessCategory, IMapper mapper)
        {
            _businessCategory = businessCategory;
            _mapper = mapper;
        }
        public async Task<Response<List<BusinessCategoryResponse>>> Handle(GetBusinessCategoryQuery request, CancellationToken cancellationToken)
        {
            var result = await _businessCategory.GetBusinessCategory();
            return new Response<List<BusinessCategoryResponse>>(result, message: "Business Category Received Successfully!!");
        }
    }
}
