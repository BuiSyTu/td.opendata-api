using Microsoft.EntityFrameworkCore;
using TD.OpenData.WebApi.Application.Catalog.Services;
using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Domain.Catalog;
using TD.OpenData.WebApi.Infrastructure.FileStorage.Interfaces;
using TD.OpenData.WebApi.Infrastructure.FileStorage.Models;
using TD.OpenData.WebApi.Infrastructure.Persistence.Contexts;

namespace TD.OpenData.WebApi.Infrastructure.FileStorage.Services;

public class SqlService : ISqlService
{
    private readonly ApplicationDbContext _context;
    private readonly IRepositoryAsync _repositoryAsync;

    public SqlService(
        ApplicationDbContext context,
        IRepositoryAsync repositoryAsync)
    {
        _context = context;
        _repositoryAsync = repositoryAsync;
    }

    public void CreateColumnSql(Dataset dataset)
    {
        throw new NotImplementedException();
    }

    public string? CreateTableSql(PreviewData previewData)
    {
        string? tableName = Guid.NewGuid().ToString().Replace("-", "_");

        string? sql = $@"IF NOT EXISTS
                    (SELECT[name]
                        FROM sys.tables
                        WHERE[name] = 'tandanjsc_{tableName}'
                    )
                    Create table tandanjsc_{tableName}(
                        ID int NOT NULL AUTO_INCREMENT,
                        {string.Join(", ", previewData.Metadata.Select(metadata => $"{metadata.Data} nvarchar(MAX)").ToArray())},
                        PRIMARY KEY (ID)
                    )
                ";

        try
        {
            _context.Database.ExecuteSqlRaw(sql);
            Console.WriteLine($"Create table tandanjsc_{tableName} successfully");
            return tableName;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.ToString());
            return null;
        }
    }

    public async void CreateTableSql(Guid datasetId)
    {
        Dataset? dataset = await _repositoryAsync.GetByIdAsync<Dataset>(datasetId);
        var previewData = dataset.Metadata.ToPreviewData();

        string? tableName = CreateTableSql(previewData);
        dataset.TableName = tableName;
    }

    public void ImportData(Dataset dataset)
    {
        throw new NotImplementedException();
    }
}
