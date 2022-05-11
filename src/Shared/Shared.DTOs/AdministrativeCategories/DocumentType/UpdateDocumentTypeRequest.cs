using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD.OpenData.WebApi.Shared.DTOs.AdministrativeCategories.DocumentType;

public class UpdateDocumentTypeRequest : IMustBeValid
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
