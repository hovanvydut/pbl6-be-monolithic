using System.Text;
using System.Text.Json;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Constants;
using Monolithic.Models.DTO;
using Monolithic.Services.Interface;
using static System.Net.Mime.MediaTypeNames;

namespace Monolithic.Services.Implement;

public class AIService : IAIService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ICapPublisher _capBus;
    public AIService(IHttpClientFactory httpClientFactory, ICapPublisher capBus)
    {
        _httpClientFactory = httpClientFactory;
        _capBus = capBus;
    }

    public async Task<AnalyseReviewResDTO> analyseReview(string reviewStrv)
    { 
        try {
            _capBus.Publish(WorkerConst.REVIEW, reviewStrv);
            return null;
        } catch (Exception e) {
            return null;
        }
    }

    public Task<AnalyseReviewResDTO> analyseReview(int reviewId)
    {
        try {
            _capBus.Publish(WorkerConst.REVIEW, reviewId);
            return null;
        } catch (Exception e) {
            return null;
        }
    }

    private HttpClient GetHttpClient()
    {
        return _httpClientFactory.CreateClient();
    }
}
