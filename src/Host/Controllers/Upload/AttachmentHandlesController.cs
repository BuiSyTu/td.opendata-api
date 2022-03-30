using Microsoft.AspNetCore.Mvc;
using TD.OpenData.WebApi.Infrastructure.FileStorage.Interfaces;

namespace TD.OpenData.WebApi.Host.Controllers.Upload;

[ApiConventionType(typeof(FSHApiConventions))]
public class AttachmentHandlesController : BaseController
{
    private readonly ITextReader _textReader;
    private readonly IExcelReader _excelReader;

    public AttachmentHandlesController(
        ITextReader textReader,
        IExcelReader excelReader)
    {
        _textReader = textReader;
        _excelReader = excelReader;
    }

    [HttpPost("text")]
    [DisableRequestSizeLimit]
    public IActionResult PostText([FromForm(Name = "file")] IFormFile file)
    {
        Stream? stream = file.OpenReadStream();
        return Ok(stream);
    }

    [HttpPost("excel")]
    [DisableRequestSizeLimit]
    public IActionResult PostExcel([FromForm(Name = "file")] IFormFile file)
    {
        Stream? stream = file.OpenReadStream();
        var xxx = _excelReader.GetMetadata(stream, 0);
        return Ok(xxx);
    }
}