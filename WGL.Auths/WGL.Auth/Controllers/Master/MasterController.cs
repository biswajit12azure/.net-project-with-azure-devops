using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WGL.Auth.Application.CQRS.Account.Queries.DocumentType;
using WGL.Auth.Application.CQRS.Masters.Queries;
using WGL.Auth.Controllers.BaseController;
using WGL.Utility.BlobFiles;

namespace WGL.Auth.Controllers.Master
{
   
    public class MasterController : BaseApiController
    {
        [HttpGet("GetPortalDetails")]
        public async Task<IActionResult> GetDocumentType()
        {
            return Ok(await Mediator.Send(new GetPortalDetailsQuery()));
        }
        [HttpGet("download")]
        public async Task<IActionResult> DownloadFile()
        {

            var response = await Mediator.Send(new DownloadFileQuery(NDAFileDownload.FileName));
            return Ok(response);

        }
    }
}
