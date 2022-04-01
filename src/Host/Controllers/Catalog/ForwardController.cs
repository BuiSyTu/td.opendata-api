using Microsoft.AspNetCore.Mvc;
using TD.OpenData.WebApi.Shared.DTOs.Forward;
using TD.OpenData.WebApi.Application.Forward;

namespace TD.OpenData.WebApi.Host.Controllers.Catalog;

[ApiConventionType(typeof(FSHApiConventions))]
public class ForwardController : BaseController
{
    private readonly IForwardService _forwardService;

    public ForwardController(IForwardService forwardService)
    {
        _forwardService = forwardService;
    }

    [HttpPost]
    public async Task<IActionResult> Forward(AxiosConfig axiosConfig)
    {
        string? result = await _forwardService.ForwardAxios(axiosConfig);
        return Ok(result);
    }
}