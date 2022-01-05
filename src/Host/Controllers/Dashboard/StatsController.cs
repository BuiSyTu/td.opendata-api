using TD.OpenData.WebApi.Application.Dashboard;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Shared.DTOs.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace TD.OpenData.WebApi.Host.Controllers.Dashboard;

[ApiConventionType(typeof(FSHApiConventions))]
public class StatsController : BaseController
{
    private readonly IStatsService _service;

    public StatsController(IStatsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<Result<StatsDto>>> GetAsync()
    {
        var stats = await _service.GetDataAsync();
        return Ok(stats);
    }
}