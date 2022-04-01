using System.ComponentModel;

namespace TD.OpenData.WebApi.Domain.Constants;

public partial class PermissionConstants
{
    [DisplayName("Identity")]
    [Description("Identity Permissions")]
    public static class Identity
    {
        public const string Register = "Permissions.Identity.Register";
    }

    [DisplayName("Roles")]
    [Description("Roles Permissions")]
    public static class Roles
    {
        public const string View = "Permissions.Roles.View";
        public const string ListAll = "Permissions.Roles.ViewAll";
        public const string Register = "Permissions.Roles.Register";
        public const string Update = "Permissions.Roles.Update";
        public const string Remove = "Permissions.Roles.Remove";
    }

    [DisplayName("Role Claims")]
    [Description("Role Claims Permissions")]
    public static class RoleClaims
    {
        public const string View = "Permissions.RoleClaims.View";
        public const string Create = "Permissions.RoleClaims.Create";
        public const string Edit = "Permissions.RoleClaims.Edit";
        public const string Delete = "Permissions.RoleClaims.Delete";
        public const string Search = "Permissions.RoleClaims.Search";
    }

    [DisplayName("Users")]
    [Description("Users Permissions")]
    public static class Users
    {
        public const string View = "Permissions.Users.View";
        public const string Create = "Permissions.Users.Create";
        public const string Edit = "Permissions.Users.Edit";
        public const string Delete = "Permissions.Users.Delete";
        public const string Export = "Permissions.Users.Export";
        public const string Search = "Permissions.Users.Search";
    }

    [DisplayName("Opendatas")]
    [Description("Opendatas Permissions")]
    public static class Opendata
    {
        public const string QTDanhMuc = "QTDanhMuc";
        public const string QTDanhMucTTHC = "QTDanhMucTTHC";
        public const string DataHTThuThap = "DataHT.ThuThap";
        public const string DataHTDuyet = "DataHT.Duyet";
        public const string DataHTKhaiThac = "DataHT.KhaiThac";
        public const string DataHTChiaSe = "DataHT.ChiaSe";
        public const string DataDVThuThap = "DataDV.ThuThap";
        public const string DataDVDuyet = "DataDV.Duyet";
        public const string DataDVKhaiThac = "DataDV.KhaiThac";
        public const string DataDVChiaSe = "DataDV.ChiaSe";
        public const string CongCanBoQTDanhMuc = "CongCanBo.QTDanhMuc";
        public const string CongCanBoTraCuuDL = "CongCanBo.TraCuuDL";
        public const string CongMoQTDanhMuc = "CongMo.QTDanhMuc";
        public const string CongMoQTTaiKhoan = "CongMo.QTTaiKhoan";
        public const string CongMoQTDuLieu = "CongMo.QTDuLieu";
    }

}