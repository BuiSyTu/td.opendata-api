using Newtonsoft.Json;
using RestSharp;
using TD.OpenData.WebApi.Application.Forward;
using TD.OpenData.WebApi.Domain.Catalog;
using TD.OpenData.WebApi.Shared.DTOs.Forward;

namespace TD.OpenData.WebApi.Infrastructure.Forward;

public class ForwardService : IForwardService
{
    public async Task<string?> ForwardAxios(AxiosConfig axiosConfig)
    {
        var client = new RestClient(axiosConfig.Url);

        var request = new RestRequest(string.Empty, StringToMethod(axiosConfig?.Method));

        if (!string.IsNullOrEmpty(axiosConfig?.Headers))
        {
            var headers = JsonConvert.DeserializeObject<Dictionary<string, string>>(axiosConfig.Headers);
            foreach (var header in headers)
            {
                request.AddHeader(header.Key, header.Value);
            }
        }

        if (!string.IsNullOrEmpty(axiosConfig.Data))
        {
            request.AddJsonBody(axiosConfig.Data, "application/json");
        }

        var respond = await client.ExecuteAsync(request);
        return respond.Content;
    }

    public async Task<string?> ForwardDataset(Dataset dataset)
    {
        DatasetAPIConfig? config = dataset.DatasetAPIConfig;

        string? url = config?.Url;
        if (!Uri.IsWellFormedUriString(url, UriKind.Absolute)) throw new Exception("Dataset url is not valid");

        var client = new RestClient(url);
        var request = new RestRequest(string.Empty, StringToMethod(config?.Method));

        if (!string.IsNullOrEmpty(config?.Headers))
        {
            var headers = JsonConvert.DeserializeObject<Dictionary<string, string>>(config.Headers);
            foreach (var header in headers)
            {
                request.AddHeader(header.Key, header.Value);
            }
        }

        if (!string.IsNullOrEmpty(config?.Data))
        {
            request.AddJsonBody(config.Data, "application/json");
        }

        var respond = await client.ExecuteAsync(request);
        return respond.Content;
    }

    private Method StringToMethod(string stringMethod)
    {
        return stringMethod switch
        {
            "GET" => Method.Get,
            "POST" => Method.Post,
            "PUT" => Method.Put,
            "DELETE" => Method.Delete,
            "PATCH" => Method.Patch,
            _ => Method.Get,
        };
    }
}
