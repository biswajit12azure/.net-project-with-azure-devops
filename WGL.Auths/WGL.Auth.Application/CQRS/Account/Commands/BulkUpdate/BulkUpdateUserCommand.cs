using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGL.Auth.Application.Interfaces.UserPortalRoleMapping;
using WGL.Auth.Domain.Entities;
using WGL.Utility.Wrappers;

namespace WGL.Auth.Application.CQRS.Account.Commands.BulkUpdate
{
    public class BulkUpdateUserCommand:IRequest<Response<int>>
    {
        public IEnumerable<BulkUpdateRequest> items { get; set; }
        public BulkUpdateUserCommand(IEnumerable<BulkUpdateRequest> requests)
        {
            items = requests;   
        }
        //public int RoleAccessMappingID { get; set; }
        //public bool IsActive { get; set; }
    }
    public class BulkUpdateUserCommandHandler : IRequestHandler<BulkUpdateUserCommand, Response<int>>
    {
        private readonly IUserPortalRoleMappingRepositoryAsync _repository;
        public BulkUpdateUserCommandHandler(IUserPortalRoleMappingRepositoryAsync repository)
        {
            _repository = repository;
        }
        public async Task<Response<int>> Handle(BulkUpdateUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _repository.BulkUpdateAccessMappings(request.items);
            return new Response<int>(result,message:"Updated Successfully");
        }
    }
}
