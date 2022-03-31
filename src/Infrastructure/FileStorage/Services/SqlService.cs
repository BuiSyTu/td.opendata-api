using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using TD.OpenData.WebApi.Application.Catalog.Services;
using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.Forward.Interfaces;
using TD.OpenData.WebApi.Domain.Catalog;
using TD.OpenData.WebApi.Infrastructure.FileStorage.Interfaces;
using TD.OpenData.WebApi.Infrastructure.Persistence.Contexts;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;

namespace TD.OpenData.WebApi.Infrastructure.FileStorage.Services;

public class SqlService : ISqlService
{
    private readonly ApplicationDbContext _context;
    private readonly IRepositoryAsync _repositoryAsync;
    private readonly IForwardService _forwardService;
    private readonly IExcelReader _excelReader;

    public SqlService(
        ApplicationDbContext context,
        IRepositoryAsync repositoryAsync,
        IForwardService forwardService,
        IExcelReader excelReader)
    {
        _context = context;
        _repositoryAsync = repositoryAsync;
        _forwardService = forwardService;
        _excelReader = excelReader;
    }

    private string? CreateTableSql(MetadataCollection metadatas)
    {
        string? tableName = $"tandanjsc_{Guid.NewGuid().ToString().Replace("-", "_")}";

        string? sql = $@"IF NOT EXISTS
                    (SELECT[name]
                        FROM sys.tables
                        WHERE[name] = '{tableName}'
                    )
                    Create table {tableName}(
                        ID int NOT NULL IDENTITY(1, 1),
                        {string.Join(", ", metadatas
                            .Where(metadata => metadata.Data.ToLower() != "id")
                            .Select(metadata => $"{metadata.Data} nvarchar(MAX)")
                            .ToArray())},
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

    private void ImportData(string? tableName, MetadataCollection metadatas, List<Dictionary<string, object>> data)
    {
        string?[]? columns = metadatas
            .Where(metadata => metadata.Data.ToLower() != "id")
            .Select(metadata => metadata.Data)
            .ToArray();
        if (columns is null) throw new Exception("Column is not null herer");

        data.ForEach(dictionary =>
        {
            string?[]? values = columns
                .Select(columnName => $"N'{dictionary[columnName].ToString()}'")
                .ToArray();

            string? sql = $@"INSERT INTO {tableName}({string.Join(", ", columns)})
                    VALUES({string.Join(", ", values)})
                ";

            try
            {
                _context.Database.ExecuteSqlRaw(sql);
                Console.WriteLine($"Create table tandanjsc_{tableName} successfully");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }
        });
    }

    public async Task SyncData(Guid datasetId)
    {
#pragma warning disable CS8603 // Possible null reference return.
        var includes = new Expression<Func<Dataset, object>>[]
        {
            x => x.DatasetAPIConfig, x => x.DatasetDBConfig, x => x.DatasetFileConfig, x => x.DataType
        };
#pragma warning restore CS8603 // Possible null reference return.
        Dataset? dataset = await _repositoryAsync.GetByIdAsync<Dataset>(datasetId, includes);

        if (dataset.DataType.Code.ToLower() == "webapi")
        {
            if (string.IsNullOrEmpty(dataset.Metadata))
            {
                // Create table
                string? dataText = await _forwardService.ForwardDataset(dataset);
                var previewData = dataText.ToPreviewData(dataset.DatasetAPIConfig.DataKey);
                string? tableName = CreateTableSql(previewData.Metadata);

                // Save table name to dataset
                dataset.TableName = tableName;
                await _repositoryAsync.SaveChangesAsync();

                // Import data
                ImportData(tableName, previewData.Metadata, previewData.Data);
            }
        }

        if (dataset.DataType.Code.ToLower() == "excel")
        {
            if (string.IsNullOrEmpty(dataset.Metadata))
            {

            }
        }
    }
}
