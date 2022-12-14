using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD.OpenData.WebApi.Shared.DTOs.AdministrativeCategories.DocumentType;

public class CreateDocumentTypeRequest : IMustBeValid
{
    public string? Name { get; set; }
    public string? Description { get; private set; }
}
