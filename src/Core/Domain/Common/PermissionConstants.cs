using System.ComponentModel;

namespace TD.OpenData.WebApi.Domain.Constants;

public partial class PermissionConstants
{
    [DisplayName("AuditLogs")]
    [Description("AuditLogs Permissions")]
    public static class AuditLogs
    {
        public const string View = "Permissions.AuditLogs.View";
    }
}