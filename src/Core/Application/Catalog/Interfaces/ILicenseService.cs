using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;

namespace TD.OpenData.WebApi.Application.Catalog.Interfaces;

public interface ILicenseService : ITransientService
{
    Task<Result<LicenseDetailsDto>> GetDetailsAsync(Guid id);


    Task<PaginatedResult<LicenseDto>> SearchAsync(LicenseListFilter filter);

    Task<Result<Guid>> CreateAsync(CreateLicenseRequest request);

    Task<Result<Guid>> UpdateAsync(UpdateLicenseRequest request, Guid id);

    Task<Result<Guid>> DeleteAsync(Guid id);
}