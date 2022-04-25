using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD.OpenData.WebApi.Shared.DTOs.AdministrativeCategories.DataSource;

public class UpdateDataSourceRequest : IMustBeValid
{
    public string? Name { get; private set; }
    public string? Description { get; private set; }
    public string? Tenant { get; set; }
}
