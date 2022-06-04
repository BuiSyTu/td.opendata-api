using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.Wrapper;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;

namespace TD.OpenData.WebApi.Application.Catalog.Interfaces;

public interface IDatasetAPIConfigService : ITransientService
{
    void DeleteByDatasetId(Guid datasetId);
}