using TD.OpenData.WebApi.Domain.Common;
using TD.OpenData.WebApi.Domain.Common.Contracts;
using TD.OpenData.WebApi.Domain.Contracts;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;

namespace TD.OpenData.WebApi.Domain.Catalog;

public class DatasetDBConfig : AuditableEntity, IMustHaveTenant
{
    public string? DBProvider { get; set; }
    public string? ConnectionString { get; set; }
    public string? DatabaseName { get; set; }
    public string? DataTable { get; set; }
    public Guid? DatasetId { get; set; }
    public virtual Dataset Dataset { get; set; } = default!;
    public string? Tenant { get; set; }

    public DatasetDBConfig()
    {
    }

    public DatasetDBConfig Update(string? dBProvider, string? connectionString, string? databaseName, string? dataTable)
    {
        if (dBProvider != null && !DBProvider.NullToString().Equals(dBProvider)) DBProvider = dBProvider;
        if (connectionString != null && !ConnectionString.NullToString().Equals(connectionString)) ConnectionString = connectionString;
        if (databaseName != null && !DatabaseName.NullToString().Equals(databaseName)) DatabaseName = databaseName;
        if (dataTable != null && !DataTable.NullToString().Equals(dataTable)) DataTable = dataTable;

        return this;
    }

    public DatasetDBConfig Update(DatasetDBConfigDto request)
    {
        if (request.DBProvider != null && !DBProvider.NullToString().Equals(request.DBProvider)) DBProvider = request.DBProvider;
        if (request.ConnectionString != null && !ConnectionString.NullToString().Equals(request.ConnectionString)) ConnectionString = request.ConnectionString;
        if (request.DatabaseName != null && !DatabaseName.NullToString().Equals(request.DatabaseName)) DatabaseName = request.DatabaseName;
        if (request.DataTable != null && !DataTable.NullToString().Equals(request.DataTable)) DataTable = request.DataTable;

        return this;
    }

    public DatasetDBConfig Update(CreateDatasetDBConfigRequest request)
    {
        if (request.DBProvider != null && !DBProvider.NullToString().Equals(request.DBProvider)) DBProvider = request.DBProvider;
        if (request.ConnectionString != null && !ConnectionString.NullToString().Equals(request.ConnectionString)) ConnectionString = request.ConnectionString;
        if (request.DatabaseName != null && !DatabaseName.NullToString().Equals(request.DatabaseName)) DatabaseName = request.DatabaseName;
        if (request.DataTable != null && !DataTable.NullToString().Equals(request.DataTable)) DataTable = request.DataTable;

        return this;
    }
}