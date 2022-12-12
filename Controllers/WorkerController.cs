using Monolithic.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Constants;
using DotNetCore.CAP;

namespace Monolithic.Controllers;

public class WorkerController : BaseController
{
    private readonly IAIService _aiService;
    public WorkerController(IAIService aiService)
    {
        _aiService = aiService;
    }

    [NonAction]
    [CapSubscribe(WorkerConst.REVIEW)]
    public async Task<bool> AnalyseSentimentalReview(int reviewId)
    {
        Console.WriteLine("Worker handle " + WorkerConst.REVIEW + ", reviewID = " + reviewId);
        return await _aiService.handleReviewWorker(reviewId);
    }
}