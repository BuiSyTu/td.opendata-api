using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD.OpenData.WebApi.Shared.DTOs;

public class UpdatedRequest
{
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
}
