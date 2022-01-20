using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.OpenData.WebApi.Infrastructure.FileStorage.Models;

namespace TD.OpenData.WebApi.Infrastructure.FileStorage.Services;

public interface IExcelReader
{
    PreviewData GetData(string? url);
    PreviewData GetData(Stream stream);
}
