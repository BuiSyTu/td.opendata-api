using Microsoft.EntityFrameworkCore;
using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Infrastructure.FileStorage.Interfaces;
using TD.OpenData.WebApi.Infrastructure.FileStorage.Models;
using TD.OpenData.WebApi.Infrastructure.Persistence.Contexts;

namespace TD.OpenData.WebApi.Infrastructure.FileStorage.Services;

public class SqlService : ISqlService
{
    private readonly ApplicationDbContext _context;

    public SqlService(ApplicationDbContext context)
    {
        _context = context;
    }

    public void CreateTableSql(PreviewData previewData)
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
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.ToString());
        }
    }
}
