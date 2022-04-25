using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD.OpenData.WebApi.Shared.DTOs.AdministrativeCategories.MiningSource;

public class MiningSourceDetailsDto : IDto
{
    public Guid Id { get; set; }
    public string? Name { get; private set; }
    public string? Description { get; private set; }
    public string? Tenant { get; set; }
}
