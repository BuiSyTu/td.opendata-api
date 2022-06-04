using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Domain.Catalog;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;

namespace TD.OpenData.WebApi.Application.Catalog.Interfaces;

public interface IDatasetAPIConfigService : ITransientService
{
    DatasetAPIConfig? GetByDatasetId(Guid datasetId);
    void DeleteByDatasetId(Guid datasetId);
}