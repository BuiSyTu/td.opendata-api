using TD.OpenData.WebApi.Application.Catalog.Interfaces;
using TD.OpenData.WebApi.Application.Common.Exceptions;
using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.Common.Specifications;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Domain.Catalog;
using TD.OpenData.WebApi.Domain.Dashboard;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;
using Microsoft.Extensions.Localization;
using Mapster;
using TD.OpenData.WebApi.Infrastructure.Persistence.Contexts;

namespace TD.OpenData.WebApi.Infrastructure.Catalog.Services;

public class BannerConfigService : IBannerConfigService
{
    private readonly ApplicationDbContext _context;

    public BannerConfigService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<BannerConfig>> GetAsync()
    {
        var bannerConfig = _context.BannerConfigs.FirstOrDefault();
        return await Result<BannerConfig>.SuccessAsync(bannerConfig);
    }

    public async Task<Result<Guid>> UpdateAsync(UpdateBannerConfigRequest request)
    {
        var bannerConfig = _context.BannerConfigs.FirstOrDefault();

        // Add first config
        if (bannerConfig is null)
        {
            bannerConfig = new BannerConfig();
            _context.BannerConfigs.Add(bannerConfig);
            _context.SaveChanges();
        }

        var updatedItem = bannerConfig.Update(request);
        _context.SaveChanges();
        return await Result<Guid>.SuccessAsync(updatedItem.Id);
    }
}