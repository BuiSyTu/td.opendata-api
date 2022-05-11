using Newtonsoft.Json;
using TD.OpenData.WebApi.Domain.Common;
using TD.OpenData.WebApi.Domain.Common.Contracts;
using TD.OpenData.WebApi.Domain.Contracts;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;

namespace TD.OpenData.WebApi.Domain.Catalog;

public class Dataset : AuditableEntity, IMustHaveTenant
{
    // Tên dữ liệu
    public string? Name { get;  set; }

    // Tiêu dề dữ liệu
    public string? Title { get; set; }

    // Mô tả
    public string? Description { get;  set; }

    // Mã dữ liệu
    public string? Code { get; set; }

    // Từ khóa dữ liệu
    public string? Tags { get; set; }

    // Trạng thái xét duyệt của dữ liệu
    // 0 = chưa duyệt
    // 1 = đã duyệt
    // 2 = bị từ chối
    public int? ApproveState { get; set; } = 0;

    // Đã đồng bộ hay chưa
    // True: Đã đồng bộ
    // False: Chưa đồng bộ
    public bool? IsSynced { get; set; } = false;

    // Public ra cổng của công dân hay không
    // True: public
    // False: private
    public bool? Visibility { get; set; } = false;

    // Giấy phép
    public Guid? LicenseId { get; set; }
    public virtual License License { get; set; } = default!;

    // Đơn vị tạo dữ liệu
    public Guid? OrganizationId { get; set; }
    public virtual Organization Organization { get; set; } = default!;

    // Loại dữ liệu
    public Guid? DataTypeId { get; set; }
    public virtual DataType? DataType { get; set; } = default!;

    // Lĩnh vực của dữ liệu
    public Guid? CategoryId { get; set; }
    public virtual Category Category { get; set; } = default!;

    // Hình thức cung cấp
    public Guid? ProviderTypeId { get; set; }
    public virtual ProviderType ProviderType { get; set; } = default!;

    // Nguồn dữ liệu
    public string? Resource { get; set; }
    public string? Metadata { get; set; }

    // Tác giả
    public string? Author { get; set; }

    // Email của tác giả
    public string? AuthorEmail { get; set; }

    // Người bảo trì
    public string? Maintainer { get; set; }

    // Email người bảo trì
    public string? MaintainerEmail { get; set; }

    public string? TableName { get; set; }

    // Đơn vị của tài khoản tạo
    public string? OfficeCode { get; set; }
    public string? OfficeName { get; set; }

    public virtual ICollection<DatasetOffice> DatasetOffices { get; set; } = new List<DatasetOffice>();
    public virtual ICollection<CustomField> CustomFields { get; set; } = new List<CustomField>();
    public virtual DatasetAPIConfig? DatasetAPIConfig { get; set; } = default!;
    public virtual DatasetFileConfig? DatasetFileConfig { get; set; } = default!;
    public virtual DatasetDBConfig? DatasetDBConfig { get; set; } = default!;

    public string? Tenant { get; set; }

    public Dataset Update(UpdateDatasetRequest request)
    {
        if (request.Name != null && !Name.NullToString().Equals(request.Name)) Name = request.Name;
        if (request.Title != null && !Title.NullToString().Equals(request.Title)) Title = request.Title;
        if (request.Description != null && !Description.NullToString().Equals(request.Description)) Description = request.Description;
        if (request.Code != null && !Code.NullToString().Equals(request.Code)) Code = request.Code;
        if (request.Tags != null && !Tags.NullToString().Equals(request.Tags)) Tags = request.Tags;
        if (request.ApproveState != null && !ApproveState.NullToString().Equals(request.ApproveState)) ApproveState = request.ApproveState;
        if (request.IsSynced != null && IsSynced != request.IsSynced) IsSynced = request.IsSynced;
        if (request.Visibility != null && Visibility != request.Visibility) Visibility = request.Visibility;

        if (request.LicenseId != Guid.Empty && !LicenseId.NullToString().Equals(request.LicenseId)) LicenseId = request.LicenseId;
        if (request.OrganizationId != Guid.Empty && !OrganizationId.NullToString().Equals(request.OrganizationId)) OrganizationId = request.OrganizationId;
        if (request.DataTypeId != Guid.Empty && !DataTypeId.NullToString().Equals(request.DataTypeId)) DataTypeId = request.DataTypeId;
        if (request.CategoryId != Guid.Empty && !CategoryId.NullToString().Equals(request.CategoryId)) CategoryId = request.CategoryId;
        if (request.ProviderTypeId != Guid.Empty && !ProviderTypeId.NullToString().Equals(request.ProviderTypeId)) ProviderTypeId = request.ProviderTypeId;

        if (request.Author != null && !Author.NullToString().Equals(request.Author)) Author = request.Author;
        if (request.AuthorEmail != null && !AuthorEmail.NullToString().Equals(request.AuthorEmail)) AuthorEmail = request.AuthorEmail;
        if (request.Maintainer != null && !Maintainer.NullToString().Equals(request.Maintainer)) Maintainer = request.Maintainer;
        if (request.MaintainerEmail != null && !MaintainerEmail.NullToString().Equals(request.MaintainerEmail)) MaintainerEmail = request.MaintainerEmail;
        if (request.Resource != null && !Resource.NullToString().Equals(request.Resource)) Resource = request.Resource;
        if (request.Metadata != null && !Metadata.NullToString().Equals(request.Metadata)) Metadata = request.Metadata;
        if (request.Tenant != null && !Tenant.NullToString().Equals(request.Tenant)) Tenant = request.Tenant;

        if (request.TableName != null && !TableName.NullToString().Equals(request.TableName)) TableName = request.TableName;

        if (request.LastModifiedBy != null && !LastModifiedBy.NullToString().Equals(request.LastModifiedBy)) LastModifiedBy = request.LastModifiedBy;
        if (request.LastModifiedOn != null && !LastModifiedOn.NullToString().Equals(request.LastModifiedOn)) LastModifiedOn = request.LastModifiedOn;
        return this;
    }

    public MetadataCollection ToMetadataCollection()
    {
        var metadatas = JsonConvert.DeserializeObject<List<Metadata>>(Metadata);
        return new MetadataCollection(metadatas);
    }
}