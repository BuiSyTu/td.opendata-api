using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Shared.DTOs.AdministrativeCategories.MiningSource;

namespace TD.OpenData.WebApi.Application.AdministrativeCategories.Interfaces;

public interface IMiningSourceService : ITransientService
{
    Task<Result<MiningSourceDetailsDto>> GetDetailsAsync(Guid id);

    Task<PaginatedResult<MiningSourceDto>> SearchAsync(MiningSourceListFilter filter);

    Task<Result<Guid>> CreateAsync(CreateMiningSourceRequest request);

    Task<Result<Guid>> UpdateAsync(UpdateMiningSourceRequest request, Guid id);

    Task<Result<Guid>> DeleteAsync(Guid id);
}
