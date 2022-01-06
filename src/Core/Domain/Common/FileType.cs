using System.ComponentModel;

namespace TD.OpenData.WebApi.Domain.Common;

public enum FileType
{
    [Description(".jpg,.png,.jpeg")]
    Image,
    [Description(".json")]
    Json,
    [Description(".doc,.docx")]
    Doc,
    [Description(".xls,.xlsx")]
    Excel
}