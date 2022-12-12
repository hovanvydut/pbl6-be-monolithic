using Monolithic.Services.Interface;
using Monolithic.Models.Entities;
using Monolithic.Models.Context;
using Monolithic.Models.DTO;
using Monolithic.Constants;
using DotNetCore.CAP;

namespace Monolithic.Services.Implement;

public class AIService : IAIService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly DataContext _db;
    private readonly ICapPublisher _capBus;
    public AIService(IHttpClientFactory httpClientFactory,
                     IConfiguration configuration,
                     DataContext db,
                     ICapPublisher capBus)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _db = db;
        _capBus = capBus;
    }

    // Handle message
    public async Task<bool> handleReviewWorker(int reviewId)
    {
        ReviewEntity reviewEntity = await _db.Reviews.FindAsync(reviewId);
        if (reviewEntity == null) return false;

        string reviewContent = reviewEntity.Content;
        var payload = new { Content = reviewContent };

        string url = _configuration["AIServiceDSN"];
        var httpClient = _httpClientFactory.CreateClient();
        var httpResponse = await httpClient.PostAsJsonAsync(url, payload);

        if (httpResponse.IsSuccessStatusCode)
        {
            var res = await httpResponse.Content.ReadFromJsonAsync<AnalyseReviewResDTO>();
            reviewEntity.Sentiment = res.Status;
            return await _db.SaveChangesAsync() > 0;
        }
        return false;
    }

    // Publish message
    public async Task<AnalyseReviewResDTO> analyseReview(string reviewStrv)
    {
        try
        {
            await _capBus.PublishAsync(WorkerConst.REVIEW, reviewStrv);
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<AnalyseReviewResDTO> analyseReview(int reviewId)
    {
        try
        {
            await _capBus.PublishAsync(WorkerConst.REVIEW, reviewId);
            return null;
        }
        catch
        {
            return null;
        }
    }
}