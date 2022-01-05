using TD.OpenData.WebApi.Domain.Common;
using TD.OpenData.WebApi.Domain.Common.Contracts;
using TD.OpenData.WebApi.Domain.Contracts;

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
    public virtual Category Parent { get; set; }
    public virtual ICollection<Category> Children { get; set; }
    public string? Tenant { get; set; }
    public virtual ICollection<Dataset> Datasets { get; set; } = new List<Dataset>();

    public Category()
    {
    }

    public Category Update(Guid? parentId, string? name, string? code, string? description, string? imageUrl, string? icon, int?order, bool? isActive)
    {
        if (name != null && !Name.NullToString().Equals(name)) Name = name;
        if (description != null && !Description.NullToString().Equals(description)) Description = description;
        if (code != null && !Code.NullToString().Equals(code)) Code = code;
        if (isActive != null && IsActive != isActive) IsActive = isActive;
        if (parentId != Guid.Empty && !ParentId.NullToString().Equals(parentId)) ParentId = parentId;
        if (imageUrl != null && !ImageUrl.NullToString().Equals(imageUrl)) ImageUrl = imageUrl;
        if (icon != null && !Icon.NullToString().Equals(icon)) Icon = icon;
        if (order != null && Order != order) Order = order;
        return this;
    }
}