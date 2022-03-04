using TD.OpenData.WebApi.Application.Catalog.Interfaces;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Domain.Constants;
using TD.OpenData.WebApi.Infrastructure.Identity.Permissions;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using TD.OpenData.WebApi.Shared.DTOs.Forward;
using TD.OpenData.WebApi.Application.Forward.Interfaces;

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