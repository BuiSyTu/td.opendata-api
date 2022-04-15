using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Application.SyncData.Interfaces;
using TD.OpenData.WebApi.Domain.Catalog;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;
using TD.OpenData.WebApi.Shared.DTOs.Filters;

namespace TD.OpenData.WebApi.Infrastructure.SyncData;

public class SqlService : ISqlService
{
    private readonly IRepositoryAsync _repository;

    public SqlService(IRepositoryAsync repository)
    {
        _repository = repository;
    }

    public async Task<string?> CreateTableSqlAsync(MetadataCollection metadatas)
    {
        string? tableName = $"tandanjsc_{Guid.NewGuid().ToString().Replace("-", "_")}";

#pragma warning disable SA1513 // Closing brace should be followed by blank line
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
                            .ToArray())
                        }, PRIMARY KEY (ID)
                    )
        ";
#pragma warning restore SA1513 // Closing brace should be followed by blank line

        try
        {
            await _repository.ExecuteSqlRawAsync(sql);
            Console.WriteLine($"Create table tandanjsc_{tableName} successfully");
            return tableName;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.ToString());
            return null;
        }
    }

    public async Task DeleteTableSqlAsync(string tableName)
    {
        try
        {
            string sql = $"DROP TABLE {tableName}";
            _ = await _repository.ExecuteSqlRawAsync(sql);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }

    public async Task<object?> GetRaw(Guid id, PaginationFilter filter)
    {
        var dataset = await _repository.GetByIdAsync<Dataset>(id);
        string? tableName = dataset.TableName;

        if (tableName is null) return null;

        string sql = $@"SELECT * FROM [OpenDataWebAPIApplication].[dbo].[{tableName}]
            ORDER BY Id DESC
            OFFSET ({(filter.PageNumber - 1) * filter.PageSize}) ROWS FETCH NEXT ({filter.PageSize}) ROWS ONLY
        ";
        var raw = await _repository.QueryAsync<object>(sql);
        return raw;
    }

    public async Task ImportDataAsync(string? tableName, MetadataCollection metadatas, List<Dictionary<string, object>> data)
    {
        string?[]? columns = metadatas
            .Where(metadata => metadata.Data.ToLower() != "id")
            .Select(metadata => metadata.Data)
            .ToArray();
        if (columns is null) throw new Exception("Column is not null herer");

        foreach (var dictionary in data)
        {
            string?[]? values = columns
                .Select(columnName => $"N'{dictionary[columnName]}'")
                .ToArray();

            string? sql = $@"INSERT INTO {tableName}({string.Join(", ", columns)})
                    VALUES({string.Join(", ", values)})
                ";

            try
            {
                _ = await _repository.ExecuteSqlRawAsync(sql);
                Console.WriteLine($"Create table tandanjsc_{tableName} successfully");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
