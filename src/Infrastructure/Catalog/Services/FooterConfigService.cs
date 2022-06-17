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

public class FooterConfigService : IFooterConfigService
{
    private readonly ApplicationDbContext _context;

    public FooterConfigService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<FooterConfig>> GetAsync()
    {
        var footerConfig = _context.FooterConfigs.FirstOrDefault();
        return await Result<FooterConfig>.SuccessAsync(footerConfig);
    }

    public async Task<Result<Guid>> UpdateAsync(UpdateFooterConfigRequest request)
    {
        var footerConfig = _context.FooterConfigs.FirstOrDefault();

        // Add first config
        if (footerConfig is null)
        {
            footerConfig = new FooterConfig();
            _context.FooterConfigs.Add(footerConfig);
            _context.SaveChanges();
        }

        var updatedItem = footerConfig.Update(request);
        _context.SaveChanges();
        return await Result<Guid>.SuccessAsync(updatedItem.Id);
    }
}