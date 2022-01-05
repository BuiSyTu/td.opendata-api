using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;

namespace TD.OpenData.WebApi.Application.Catalog.Interfaces;

public interface IDataTypeService : ITransientService
{
    Task<Result<DataTypeDetailsDto>> GetDetailsAsync(Guid id);


    Task<PaginatedResult<DataTypeDto>> SearchAsync(DataTypeListFilter filter);

    Task<Result<Guid>> CreateAsync(CreateDataTypeRequest request);

    Task<Result<Guid>> UpdateAsync(UpdateDataTypeRequest request, Guid id);

    Task<Result<Guid>> DeleteAsync(Guid id);
}