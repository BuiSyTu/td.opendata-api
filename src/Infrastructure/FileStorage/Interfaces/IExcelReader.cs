using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.OpenData.WebApi.Application.Common.Interfaces;

namespace TD.OpenData.WebApi.Infrastructure.FileStorage.Interfaces;

public interface IExcelReader : ITransientService
{
    string? GetJsonData(Stream? stream, string? sheetName);
}
