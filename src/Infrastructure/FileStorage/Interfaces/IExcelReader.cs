using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Infrastructure.FileStorage.Models;

namespace TD.OpenData.WebApi.Infrastructure.FileStorage.Interfaces;

public interface IExcelReader : ITransientService
{
    PreviewData GetData(string? url);
    PreviewData GetData(Stream? stream, int? sheetNumber);
    PreviewData GetData(Stream? stream, string? sheetName);

    MetadataCollection GetMetadata(Stream? stream, int? sheetNumber);
    MetadataCollection GetMetadata(Stream? stream, string? sheetNumber);
}
