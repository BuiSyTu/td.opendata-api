using TD.OpenData.WebApi.Application.Catalog.Interfaces;
using TD.OpenData.WebApi.Domain.Catalog;
using TD.OpenData.WebApi.Infrastructure.Persistence.Contexts;

namespace TD.OpenData.WebApi.Infrastructure.Catalog.Services;

public class DatasetFileConfigService : IDatasetFileConfigService
{
    private readonly ApplicationDbContext _dbContext;

    public DatasetFileConfigService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public void DeleteByDatasetId(Guid datasetId)
    {
        var item = _dbContext.DatasetFileConfigs.FirstOrDefault(x => x.DatasetId == datasetId);
        if (item != null)
        {
            _dbContext.DatasetFileConfigs.Remove(item);
            _dbContext.SaveChanges();
        }
    }

    public DatasetFileConfig? GetByDatasetId(Guid datasetId)
    {
        return _dbContext.DatasetFileConfigs.FirstOrDefault(x => x.DatasetId == datasetId);
    }
}