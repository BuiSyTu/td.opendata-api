using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Shared.DTOs.AdministrativeCategories.DataSource;

namespace TD.OpenData.WebApi.Application.AdministrativeCategories.Interfaces;

public interface IDataSourceService : ITransientService
{
    Task<Result<DataSourceDetailsDto>> GetDetailsAsync(Guid id);

    Task<PaginatedResult<DataSourceDto>> SearchAsync(DataSourceListFilter filter);

    Task<Result<Guid>> CreateAsync(CreateDataSourceRequest request);

    Task<Result<Guid>> UpdateAsync(UpdateDataSourceRequest request, Guid id);

    Task<Result<Guid>> DeleteAsync(Guid id);
}
