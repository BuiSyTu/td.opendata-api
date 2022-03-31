using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Shared.DTOs.Catalog;

namespace TD.OpenData.WebApi.Infrastructure.FileStorage.Interfaces;

public interface ITextReader : ITransientService
{
    Task<PreviewData> GetData(string? url);
    PreviewData GetData(Stream? stream);
}
