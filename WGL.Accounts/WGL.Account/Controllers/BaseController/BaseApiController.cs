using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WGL.Account.Controllers.BaseController
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public abstract class BaseApiController : ControllerBase
    {
        private IMediator? _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>() ?? 
            throw new InvalidOperationException("IMediator service not found.");
    }
}
