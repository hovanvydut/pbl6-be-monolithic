using Monolithic.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Constants;
using Monolithic.Helpers;
using DotNetCore.CAP;

namespace Monolithic.Controllers;

public class WorkerController : BaseController
{
    private readonly IAIService _aiService;
    private readonly ISendMailHelper _sendMailHelper;
    private readonly ILogger<WorkerController> _logger;
    public WorkerController(IAIService aiService,
                            ISendMailHelper sendMailHelper,
                            ILogger<WorkerController> logger)
    {
        _aiService = aiService;
        _sendMailHelper = sendMailHelper;
        _logger = logger;
    }

    [NonAction]
    [CapSubscribe(WorkerConst.REVIEW)]
    public async Task<bool> AnalyseSentimentalReview(int reviewId)
    {
        _logger.LogInformation($"Worker handle {WorkerConst.REVIEW} has reviewID = {reviewId}");
        return await _aiService.handleReviewWorker(reviewId);
    }

    [NonAction]
    [CapSubscribe(WorkerConst.SEND_MAIL)]
    public async Task SendMail(MailContent mailContent)
    {
        _logger.LogInformation($"Worker handle {WorkerConst.SEND_MAIL} to = {mailContent.ToEmail}");
        await _sendMailHelper.SendEmailAsync(mailContent);
    }
}