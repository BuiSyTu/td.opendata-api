using Microsoft.AspNetCore.Mvc;
using TD.OpenData.WebApi.Infrastructure.FileStorage.Interfaces;

namespace TD.OpenData.WebApi.Host.Controllers.FileToJson;

[ApiConventionType(typeof(FSHApiConventions))]
public class AttachmentHandlesController : BaseController
{
    private readonly IExcelReader _excelReader;
    private readonly ITextReader _textReader;
    private readonly ISqlService _sqlService;

    public AttachmentHandlesController(
        IExcelReader excelReader,
        ITextReader textReader,
        ISqlService sqlService
    )
    {
        _excelReader = excelReader;
        _textReader = textReader;
        _sqlService = sqlService;
    }

    [HttpPost("text")]
    [DisableRequestSizeLimit]
    public IActionResult PostText([FromForm(Name = "file")] IFormFile file)
    {
        Stream? stream = file.OpenReadStream();
        var a = _textReader.GetData(stream);

        // _sqlService.CreateTableSql(a);
        return Ok(a);
    }

    [HttpPost("excel")]
    [DisableRequestSizeLimit]
    public IActionResult PostExcel(
        [FromForm(Name = "file")] IFormFile file,
        [FromForm] int? sheetNumber = null,
        [FromForm] string? sheetName = null
    )
    {
        Stream? stream = file.OpenReadStream();
        var a = _excelReader.GetData(stream, sheetNumber);
        return Ok(a);
    }
}