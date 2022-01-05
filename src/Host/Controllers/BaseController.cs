using Microsoft.AspNetCore.Mvc;

namespace TD.OpenData.WebApi.Host.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class BaseController : ControllerBase
{
}