using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD.OpenData.WebApi.Shared.DTOs.Dashboard;
public class OverViewWidget
{
    public int? Dataset { get; set; }
    public int? Organization { get; set; }
    public int? Category { get; set; }
    public int? ProviderType { get; set; }
    public int? DataType { get; set; }
    public int? View { get; set; }
}
