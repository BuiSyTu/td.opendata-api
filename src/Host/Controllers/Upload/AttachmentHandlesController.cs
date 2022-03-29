using Microsoft.AspNetCore.Mvc;
using TD.OpenData.WebApi.Infrastructure.FileStorage.Interfaces;

namespace TD.OpenData.WebApi.Host.Controllers.Upload;

[ApiConventionType(typeof(FSHApiConventions))]
public class AttachmentHandlesController : BaseController
{
    public AttachmentHandlesController()
    {
    }

    [HttpPost("text")]
    [DisableRequestSizeLimit]
    public IActionResult PostText([FromForm(Name = "file")] IFormFile file)
    {
        Stream? stream = file.OpenReadStream();
        return Ok(stream);
    }
}