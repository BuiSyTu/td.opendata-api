using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD.OpenData.WebApi.Infrastructure.FileStorage.Models;

public class PreviewData
{
    public JObject? Data { get; set; }
    public string? DataSource { get; set; }
    public MetadataCollection? Metadata { get; set; }
}
