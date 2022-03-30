using System.ComponentModel;
using TD.OpenData.WebApi.Application.Common.Interfaces;

namespace TD.OpenData.WebApi.Application.Catalog.Interfaces;

public interface IDatasetJob : IScopedService
{
    [DisplayName("Đồng bộ dữ liệu WebAPI")]
    Task DatasetWebAPIAsync(Guid idDataset);

    [DisplayName("Đồng bộ dữ liệu Database")]
    Task DatasetDatabaseAsync(Guid idDataset);

    [DisplayName("Đồng bộ dữ liệu File")]
    Task DatasetFileAsync(Guid idDataset);

    [DisplayName("removes all radom brands created example job on Queue notDefault")]
    Task CleanAsync();
}