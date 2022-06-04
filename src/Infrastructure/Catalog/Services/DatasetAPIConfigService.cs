using TD.OpenData.WebApi.Application.Catalog.Interfaces;
using TD.OpenData.WebApi.Infrastructure.Persistence.Contexts;

namespace TD.OpenData.WebApi.Infrastructure.Catalog.Services;

public class DatasetAPIConfigService : IDatasetAPIConfigService
{
    private readonly ApplicationDbContext _dbContext;

    public DatasetAPIConfigService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public void DeleteByDatasetId(Guid datasetId)
    {
        var item = _dbContext.DatasetAPIConfigs.FirstOrDefault(x => x.DatasetId == datasetId);
        if (item != null)
        {
            _dbContext.DatasetAPIConfigs.Remove(item);
            _dbContext.SaveChanges();
        }
    }
}