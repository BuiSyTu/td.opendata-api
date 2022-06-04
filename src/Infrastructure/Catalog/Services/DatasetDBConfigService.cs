using TD.OpenData.WebApi.Application.Catalog.Interfaces;
using TD.OpenData.WebApi.Domain.Catalog;
using TD.OpenData.WebApi.Infrastructure.Persistence.Contexts;

namespace TD.OpenData.WebApi.Infrastructure.Catalog.Services;

public class DatasetDBConfigService : IDatasetDBConfigService
{
    private readonly ApplicationDbContext _dbContext;

    public DatasetDBConfigService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public void DeleteByDatasetId(Guid datasetId)
    {
        var item = _dbContext.DatasetDBConfigs.FirstOrDefault(x => x.DatasetId == datasetId);
        if (item != null)
        {
            _dbContext.DatasetDBConfigs.Remove(item);
            _dbContext.SaveChanges();
        }
    }

    public DatasetDBConfig? GetByDatasetId(Guid datasetId)
    {
        return _dbContext.DatasetDBConfigs.FirstOrDefault(x => x.DatasetId == datasetId);
    }
}