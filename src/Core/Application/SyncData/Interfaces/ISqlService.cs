using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;
using TD.OpenData.WebApi.Shared.DTOs.Filters;

namespace TD.OpenData.WebApi.Application.SyncData.Interfaces;

public interface ISqlService : ITransientService
{
    Task<string?> CreateTableSqlAsync(MetadataCollection metadatas);
    Task ImportDataAsync(string? tableName, MetadataCollection metadatas, List<Dictionary<string, object>> data);
    Task<object?> GetRaw(Guid id, PaginationFilter filter);
    Task DeleteTableSqlAsync(string tableName);
}
