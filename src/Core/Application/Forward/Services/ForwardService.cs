using Newtonsoft.Json;
using RestSharp;
using TD.OpenData.WebApi.Application.Forward.Interfaces;
using TD.OpenData.WebApi.Shared.DTOs.Forward;

namespace TD.OpenData.WebApi.Application.Forward.Services;

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

    private Method StringToMethod(string stringMethod)
    {
        return stringMethod switch
        {
            "GET" => Method.Get,
            "POST" => Method.Post,
            "PUT" => Method.Put,
            "DELETE" => Method.Delete,
            _ => Method.Get,
        };
    }
}
