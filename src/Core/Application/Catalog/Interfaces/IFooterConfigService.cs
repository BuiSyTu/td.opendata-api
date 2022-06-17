using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Domain.Catalog;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;

namespace TD.OpenData.WebApi.Application.Catalog.Interfaces;

public interface IFooterConfigService : ITransientService
{
    Task<Result<FooterConfig>> GetAsync();
    Task<Result<Guid>> UpdateAsync(UpdateFooterConfigRequest request);
}