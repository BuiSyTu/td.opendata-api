using TD.OpenData.WebApi.Domain.Common;
using TD.OpenData.WebApi.Domain.Common.Contracts;
using TD.OpenData.WebApi.Domain.Contracts;

namespace TD.OpenData.WebApi.Domain.Catalog;

public class DatasetFileConfig : AuditableEntity, IMustHaveTenant
{
    public string? FileType { get;  set; }
    public string? FileName { get;  set; }
    public string? FileUrl { get; set; }
    public string? SheetName { get; set; }
    public string? Tenant { get; set; }
    public Guid? DatasetId { get; set; }

    public virtual Dataset Dataset { get; set; } = default!;

    public DatasetFileConfig Update(string? fileType, string? fileName, string? fileUrl, string? sheetName, Guid? datasetId)
    {
        if (fileType != null && !FileType.NullToString().Equals(fileType)) FileType = fileType;
        if (fileName != null && !FileName.NullToString().Equals(fileName)) FileName = fileName;
        if (fileUrl != null && !FileUrl.NullToString().Equals(fileUrl)) FileUrl = fileUrl;
        if (sheetName != null && !SheetName.NullToString().Equals(sheetName)) SheetName = sheetName;
        if (datasetId != Guid.Empty && !DatasetId.NullToString().Equals(datasetId)) DatasetId = datasetId;
        return this;
    }
}