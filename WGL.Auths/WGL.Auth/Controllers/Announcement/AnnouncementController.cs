using Microsoft.AspNetCore.Mvc;
using WGL.Auth.Application.CQRS.Account.Queries.GetUserLoginToken;
using WGL.Auth.Controllers.BaseController;

namespace WGL.Auth.Controllers.Announcement
{
    public class AnnouncementController : BaseApiController
    {
        [HttpGet("GetAnnouncement")]
        public async Task<IActionResult> GetAnnouncement(string UserName, string Password)
        {
            return Ok();
        }
    }
}
