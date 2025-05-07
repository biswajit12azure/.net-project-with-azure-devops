using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WGL.Auth.Application.CQRS.Account.Commands.BulkUpdate;
using WGL.Auth.Application.CQRS.Account.Queries.Mapping;
using WGL.Auth.Controllers.BaseController;
using WGL.Auth.Domain.Entities;

namespace WGL.Auth.Controllers.GetUserPortRole
{

    public class UserPortalRoleMappingController : BaseApiController
    {
        //Session Login Token
        [HttpGet("GetUserPortalRoleMapping")]
        public async Task<IActionResult> GetUserPortalRoleMapping(int PortalID,int RoleAccessMappingID,int AccessID, int RoleID,bool IsActive)
        {
            return Ok(await Mediator.Send(new GetUserPortalRoleMappingQuery() 
                            {PortalID=PortalID, 
                                RoleAccessMappingID= RoleAccessMappingID, 
                                AccessID = AccessID, 
                                RoleID = RoleID, 
                                IsActive= IsActive }));
        }
        [HttpPut]
        public async Task<IActionResult> BulkUpdateUserPortalRoleMapping(List<BulkUpdateRequest> command)
        {
            if(command == null || !command.Any())
            {
                return BadRequest("Request Body is Empty or Invalid");
            }
            var request = new BulkUpdateUserCommand(command);
            var result = await Mediator.Send(request);
            
            return Ok(result);
        }
    }

}
