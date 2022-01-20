using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.OpenData.WebApi.Infrastructure.FileStorage.Models;

namespace TD.OpenData.WebApi.Infrastructure.FileStorage.Services;

public class ExcelReader : IExcelReader
{
    public PreviewData GetData(string? url)
    {
        throw new NotImplementedException();
    }

    public PreviewData GetData(Stream stream)
    {
        throw new NotImplementedException();
    }
}
