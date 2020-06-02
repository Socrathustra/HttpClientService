using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HttpClientService
{
    public interface IHttpClientService
    {
        Task<TContentType> GetAsync<TContentType>(string baseUrl, Dictionary<string, string> queryParameters = null);
        Task<TContentType> PostAsync<TContentType>(Uri url, object body, JsonSerializerOptions options = null);
    }

    public class HttpClientService : IHttpClientService
    {
        private HttpClient HttpClient { get; set; }

        public HttpClientService(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
        }

        public async Task<TContentType> GetAsync<TContentType>(string baseUrl, Dictionary<string, string> queryParameters = null)
        {
            var assembledUrl = baseUrl;

            if (queryParameters?.Count > 0)
            {
                assembledUrl = HttpClientService.AssembleQuerystring(baseUrl, queryParameters);
            }

            var response = await this.HttpClient.GetAsync(new Uri(assembledUrl)).ConfigureAwait(false);

            return await response.GetDeserializedContentAsync<TContentType>();
        }

        public async Task<TContentType> PostAsync<TContentType>(Uri url, object body, JsonSerializerOptions options = null)
        {
            using (var stringContent = new StringContent(JsonSerializer.Serialize(body, options)))
            {
                return await (await this.HttpClient.PostAsync(url, stringContent).ConfigureAwait(false)).GetDeserializedContentAsync<TContentType>();
            }
        }

        private static string AssembleQuerystring(string url, Dictionary<string, string> queryParameters)
        {
            var outputUrlBuilder = new StringBuilder(url);
            outputUrlBuilder.Append("?");

            var firstParameter = true;
            foreach (var param in queryParameters)
            {
                if (firstParameter == false)
                {
                    outputUrlBuilder.Append(",");
                }
                outputUrlBuilder.Append($"{param.Key}={param.Value}");

                firstParameter = false;
            }

            return outputUrlBuilder.ToString();
        }
    }
}
