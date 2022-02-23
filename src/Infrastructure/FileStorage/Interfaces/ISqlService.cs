using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Infrastructure.FileStorage.Models;

namespace TD.OpenData.WebApi.Infrastructure.FileStorage.Interfaces;

public interface ISqlService : ITransientService
{
    void CreateTableSql(PreviewData previewData);
}
