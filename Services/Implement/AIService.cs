using System.Text;
using System.Text.Json;
using Monolithic.Models.DTO;
using Monolithic.Services.Interface;
using static System.Net.Mime.MediaTypeNames;

namespace Monolithic.Services.Implement;

public class AIService : IAIService
{
    private readonly IHttpClientFactory _httpClientFactory;
    public AIService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<AnalyseReviewResDTO> analyseReview(string reviewStrv)
    { 
        try {
        var payload = new {Content = reviewStrv};

        string url = "http://node-4.silk-cat.software:3333/analyse-comment";
        var httpClient = GetHttpClient();
        var httpResponseMessage = await httpClient.PostAsJsonAsync(url, payload);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var res =
                await httpResponseMessage.Content.ReadFromJsonAsync<AnalyseReviewResDTO>();
            
            return res;
        }
        return null;
        } catch (Exception e) {
            return null;
        }
    }

    public Task<AnalyseReviewResDTO> analyseReview(int reviewId)
    {
        throw new NotImplementedException();
    }

    private HttpClient GetHttpClient()
    {
        return _httpClientFactory.CreateClient();
    }
}
