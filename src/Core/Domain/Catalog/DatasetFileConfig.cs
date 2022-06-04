using TD.OpenData.WebApi.Domain.Common;
using TD.OpenData.WebApi.Domain.Common.Contracts;
using TD.OpenData.WebApi.Domain.Contracts;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;

namespace TD.OpenData.WebApi.Domain.Catalog;

public class DatasetFileConfig : AuditableEntity, IMustHaveTenant
{
    public string? FileType { get; set; }
    public string? FileName { get; set; }
    public string? FileUrl { get; set; }
    public string? SheetName { get; set; }
    public string? Tenant { get; set; }
    public Guid? DatasetId { get; set; }

    public virtual Dataset Dataset { get; set; } = default!;

    public DatasetFileConfig Update(string? fileType, string? fileName, string? fileUrl, string? sheetName)
    {
        if (fileType != null && !FileType.NullToString().Equals(fileType)) FileType = fileType;
        if (fileName != null && !FileName.NullToString().Equals(fileName)) FileName = fileName;
        if (fileUrl != null && !FileUrl.NullToString().Equals(fileUrl)) FileUrl = fileUrl;
        if (sheetName != null && !SheetName.NullToString().Equals(sheetName)) SheetName = sheetName;

        return this;
    }

    public DatasetFileConfig Update(DatasetFileConfigDto request)
    {
        if (request.FileType != null && !FileType.NullToString().Equals(request.FileType)) FileType = request.FileType;
        if (request.FileName != null && !FileName.NullToString().Equals(request.FileName)) FileName = request.FileName;
        if (request.FileUrl != null && !FileUrl.NullToString().Equals(request.FileUrl)) FileUrl = request.FileUrl;
        if (request.SheetName != null && !SheetName.NullToString().Equals(request.SheetName)) SheetName = request.SheetName;

        return this;
    }

    public DatasetFileConfig Update(CreateDatasetFileConfigRequest request)
    {
        if (request.FileType != null && !FileType.NullToString().Equals(request.FileType)) FileType = request.FileType;
        if (request.FileName != null && !FileName.NullToString().Equals(request.FileName)) FileName = request.FileName;
        if (request.FileUrl != null && !FileUrl.NullToString().Equals(request.FileUrl)) FileUrl = request.FileUrl;
        if (request.SheetName != null && !SheetName.NullToString().Equals(request.SheetName)) SheetName = request.SheetName;

        return this;
    }
}