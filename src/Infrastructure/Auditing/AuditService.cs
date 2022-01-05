using TD.OpenData.WebApi.Application.Auditing;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Infrastructure.Persistence.Contexts;
using TD.OpenData.WebApi.Shared.DTOs.Auditing;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace TD.OpenData.WebApi.Infrastructure.Auditing;

public class AuditService : IAuditService
{
    private readonly ApplicationDbContext _context;

    public AuditService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IResult<IEnumerable<AuditResponse>>> GetUserTrailsAsync(string userId)
    {
        var trails = await _context.AuditTrails.Where(a => a.UserName == userId).OrderByDescending(a => a.Id).Take(250).ToListAsync();
        var mappedLogs = trails.Adapt<IEnumerable<AuditResponse>>();
        return await Result<IEnumerable<AuditResponse>>.SuccessAsync(mappedLogs);
    }
}