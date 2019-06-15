using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace chess.webapi.client.csharp
{
    public abstract class ApiClientBase
    {
        protected string BaseUrl;
        protected readonly HttpClient HttpClient;
        private string _prefix = "api";

        protected ApiClientBase(HttpClient httpClient, string baseUrl)
        {
            HttpClient = httpClient;
            BaseUrl = baseUrl;
        }

        protected async Task<string> GetStringAsync(string apiEndPoint) => await HttpClient.GetStringAsync(BuildEndpointUrl(apiEndPoint));
        protected async Task<T> GetJsonAsync<T>(string apiEndPoint) => await HttpClient.GetJsonAsync<T>(BuildEndpointUrl(apiEndPoint));

        private string BuildEndpointUrl(string apiEndPoint)
        {
            return new UriBuilder(BaseUrl)
            {
                Path = $"/{_prefix}/{apiEndPoint}"
            }.Uri.AbsoluteUri;
        }
    }
}