using TD.OpenData.WebApi.Application.Common.Interfaces;
using TD.OpenData.WebApi.Domain.Catalog;
using TD.OpenData.WebApi.Shared.DTOs.Forward;

namespace TD.OpenData.WebApi.Application.Forward;

public interface IForwardService : ITransientService
{
    public Task<string?> ForwardAxios(AxiosConfig axiosConfig);
    public Task<string?> ForwardDataset(Dataset dataset);
}
