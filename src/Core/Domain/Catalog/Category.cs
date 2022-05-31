using TD.OpenData.WebApi.Domain.Common;
using TD.OpenData.WebApi.Domain.Common.Contracts;
using TD.OpenData.WebApi.Domain.Contracts;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;

namespace TD.OpenData.WebApi.Domain.Catalog;

public class Category : AuditableEntity, IMustHaveTenant
{
    public string? Name { get;  set; }
    public string? Description { get;  set; }
    public string? Code { get; set; }
    public bool? IsActive { get;  set; }
    public string? ImageUrl { get; set; }
    public string? Icon { get; set; }
    public int? Order { get; set; }
    public Guid? ParentId { get; private set; }
    public virtual Category? Parent { get; set; }
    public virtual ICollection<Category> Children { get; set; } = new List<Category>();
    public string? Tenant { get; set; }
    public virtual ICollection<Dataset> Datasets { get; set; } = new List<Dataset>();

    public Category Update(UpdateCategoryRequest request)
    {
        if (request.Name != null && !Name.NullToString().Equals(request.Name)) Name = request.Name;
        if (request.Description != null && !Description.NullToString().Equals(request.Description)) Description = request.Description;
        if (request.Code != null && !Code.NullToString().Equals(request.Code)) Code = request.Code;
        if (request.IsActive != null && IsActive != request.IsActive) IsActive = request.IsActive;
        if (request.ParentId != Guid.Empty && !ParentId.NullToString().Equals(request.ParentId)) ParentId = request.ParentId;
        if (request.ImageUrl != null && !ImageUrl.NullToString().Equals(request.ImageUrl)) ImageUrl = request.ImageUrl;
        if (request.Icon != null && !Icon.NullToString().Equals(request.Icon)) Icon = request.Icon;
        if (request.Order != null && Order != request.Order) Order = request.Order;
        return this;
    }
}