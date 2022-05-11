using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD.OpenData.WebApi.Shared.DTOs.AdministrativeCategories.MiningSource;

public class CreateMiningSourceRequest : IMustBeValid
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
