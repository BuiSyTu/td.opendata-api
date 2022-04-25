using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.OpenData.WebApi.Domain.Common;
using TD.OpenData.WebApi.Domain.Common.Contracts;
using TD.OpenData.WebApi.Domain.Contracts;

namespace TD.OpenData.WebApi.Domain.AdministrativeCategories;

public class MiningSource : AuditableEntity, IMustHaveTenant
{
    public string? Name { get; private set; }
    public string? Description { get; private set; }
    public string? Tenant { get; set; }

    public MiningSource Update(string? name, string? description)
    {
        if (name != null && !Name.NullToString().Equals(name)) Name = name;
        if (description != null && !Description.NullToString().Equals(description)) Description = description;
        return this;
    }
}
