using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using Monolithic.Models.Common;

namespace Monolithic.Helpers;

public class HttpHelper<T> : IHttpHelper<T>
{
    private readonly IHttpClientFactory _httpClientFactory;
    public HttpHelper(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<T> GetAsync(string url, string token = "")
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var client = _httpClientFactory.CreateClient();
        if (!string.IsNullOrWhiteSpace(token))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<BaseResponse<T>>(jsonString);
            return obj.Data;
        }
        return default(T);
    }

    public async Task<bool> CreateAsync(string url, T obj, string token = "")
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        if (obj != null)
        {
            request.Content = new StringContent(
                    JsonConvert.SerializeObject(obj),
                    Encoding.UTF8,
                    "application/json");
        }
        else return false;

        var client = _httpClientFactory.CreateClient();
        if (!string.IsNullOrWhiteSpace(token))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
            return true;
        else return false;
    }

    public async Task<bool> UpdateAsync(string url, T obj, string token = "")
    {
        var request = new HttpRequestMessage(HttpMethod.Patch, url);
        if (obj != null)
        {
            request.Content = new StringContent(
                    JsonConvert.SerializeObject(obj),
                    Encoding.UTF8,
                    "application/json");
        }
        else return false;

        var client = _httpClientFactory.CreateClient();
        if (!string.IsNullOrWhiteSpace(token))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
            return true;
        else return false;
    }

    public async Task<bool> DeleteAsync(string url, string token = "")
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, url);
        var client = _httpClientFactory.CreateClient();
        if (!string.IsNullOrWhiteSpace(token))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await client.SendAsync(request);
        if (response.StatusCode == HttpStatusCode.OK)
            return true;
        else return false;
    }
}