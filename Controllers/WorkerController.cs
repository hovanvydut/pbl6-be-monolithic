using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Constants;
using Monolithic.Models.Context;
using Monolithic.Models.DTO;
using Monolithic.Models.Entities;

namespace Monolithic.Controllers;

public class WorkerController : BaseController
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly DataContext _db;
    public WorkerController(IHttpClientFactory httpClientFactory,
                            DataContext db)
    {
        _httpClientFactory = httpClientFactory;
        _db = db;
    }

    // [NonAction]
    // [CapSubscribe(WorkerConst.REVIEW)]
    // public async void AnalyseSentimentalReview(string reviewContent)
    // {
    //     var payload = new {Content = reviewContent};

    //     string url = "http://node-4.silk-cat.software:3333/analyse-comment";
    //     var httpClient = GetHttpClient();
    //     var httpResponseMessage = await httpClient.PostAsJsonAsync(url, payload);

    //     if (httpResponseMessage.IsSuccessStatusCode)
    //     {
    //         var res =
    //             await httpResponseMessage.Content.ReadFromJsonAsync<AnalyseReviewResDTO>();
            
    //         Console.WriteLine("aaaabb22");
    //         Console.WriteLine(res);
    //     }
    // }

    [NonAction]
    [CapSubscribe(WorkerConst.REVIEW)]
    public async Task<VNPHistoryDTO> AnalyseSentimentalReview(int reviewId)
    {
        Console.WriteLine("Worker handle " + WorkerConst.REVIEW + ", reviewID = " + reviewId);
        ReviewEntity reviewEntity = await _db.Reviews.FindAsync(reviewId);
        if (reviewEntity == null) return null;

        string reviewContent = reviewEntity.Content;
        var payload = new {Content = reviewContent};

        string url = "http://node-4.silk-cat.software:3333/analyse-comment";
        var httpClient = GetHttpClient();
        var httpResponseMessage = await httpClient.PostAsJsonAsync(url, payload);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var res =
                await httpResponseMessage.Content.ReadFromJsonAsync<AnalyseReviewResDTO>();
            
            Console.WriteLine("aaaabb22");
            Console.WriteLine(res.Status);
            reviewEntity.Sentiment = res.Status;
            await _db.SaveChangesAsync();
        }

        return null;
    }

    private HttpClient GetHttpClient()
    {
        return _httpClientFactory.CreateClient();
    }
}