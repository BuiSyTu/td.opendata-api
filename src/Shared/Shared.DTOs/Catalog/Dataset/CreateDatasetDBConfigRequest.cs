namespace TD.OpenData.WebApi.Shared.DTOs.Catalog;

public class CreateDatasetDBConfigRequest : IMustBeValid
{
    public string? DBProvider { get; set; }
    public string? ConnectionString { get; set; }
    public string? DatabaseName { get; set; }
    public string? DataTable { get; set; }
}