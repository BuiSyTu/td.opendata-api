using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD.OpenData.WebApi.Shared.DTOs.Forward;

public class AxiosConfig
{
    public string? Method { get; set; }
    public string? Url { get; set; }
    public string? Headers { get; set; }
    public string? Data { get; set; }
}
