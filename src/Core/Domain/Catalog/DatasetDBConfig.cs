using TD.OpenData.WebApi.Domain.Common;
using TD.OpenData.WebApi.Domain.Common.Contracts;
using TD.OpenData.WebApi.Domain.Contracts;

namespace TD.OpenData.WebApi.Domain.Catalog;

public class DatasetDBConfig : AuditableEntity, IMustHaveTenant
{
    public string? DBProvider { get;  set; }
    public string? ConnectionString { get;  set; }
    public string? DatabaseName { get; set; }
    public string? DataTable { get;  set; }
    public string? TableName { get; set; }
    public Guid? DatasetId { get; set; }
    public virtual Dataset Dataset { get; set; } = default!;
    public string? Tenant { get; set; }

    public DatasetDBConfig()
    {
    }

    public DatasetDBConfig Update(string? dBProvider, string? connectionString, string? databaseName, string? dataTable, Guid? datasetId)
    {
        if (dBProvider != null && !DBProvider.NullToString().Equals(dBProvider)) DBProvider = dBProvider;
        if (connectionString != null && !ConnectionString.NullToString().Equals(connectionString)) ConnectionString = connectionString;
        if (databaseName != null && !DatabaseName.NullToString().Equals(databaseName)) DatabaseName = databaseName;
        if (dataTable != null && !DataTable.NullToString().Equals(dataTable)) DataTable = dataTable;
        if (datasetId != Guid.Empty && !DatasetId.NullToString().Equals(datasetId)) DatasetId = datasetId;

        return this;
    }


}