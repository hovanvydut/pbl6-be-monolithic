using Monolithic.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Models.DTO;
using Monolithic.Constants;
using Monolithic.Helpers;
using DotNetCore.CAP;

namespace Monolithic.Controllers;

public class WorkerController : BaseController
{
    private readonly IAIService _aiService;
    private readonly ISendMailHelper _sendMailHelper;
    private readonly INotificationService _notiService;
    private readonly ILogger<WorkerController> _logger;
    public WorkerController(IAIService aiService,
                            ISendMailHelper sendMailHelper,
                            INotificationService notiService,
                            ILogger<WorkerController> logger)
    {
        _aiService = aiService;
        _sendMailHelper = sendMailHelper;
        _notiService = notiService;
        _logger = logger;
    }

    // REVIEW
    [NonAction]
    [CapSubscribe(WorkerConst.REVIEW)]
    public async Task<bool> AnalyseSentimentalReview(int reviewId)
    {
        _logger.LogInformation($"Worker handle {WorkerConst.REVIEW} has reviewID = {reviewId}");
        return await _aiService.handleReviewWorker(reviewId);
    }

    // MAIL
    [NonAction]
    [CapSubscribe(WorkerConst.SEND_MAIL)]
    public async Task SendMail(MailContent mailContent)
    {
        _logger.LogInformation($"Worker handle {WorkerConst.SEND_MAIL} to = {mailContent.ToEmail}");
        await _sendMailHelper.SendEmailAsync(mailContent);
    }
    
    // NOTIFICATION
    [NonAction]
    [CapSubscribe(WorkerConst.REVIEW_NOTIFICATION)]
    public async Task<bool> PushReviewNotification(ReviewNotificationDTO createDTO)
    {
        return await _notiService.CreateReviewOnPostNoty(createDTO);
    }

    [NonAction]
    [CapSubscribe(WorkerConst.BOOKING_NOTIFICATION)]
    public async Task<bool> PushBookingNotification(BookingNotificationDTO createDTO)
    {
        return await _notiService.CreateBookingOnPostNoty(createDTO);
    }

    [NonAction]
    [CapSubscribe(WorkerConst.APPROVE_MEETING_NOTIFICATION)]
    public async Task<bool> PushApproveMeetingNotification(ApproveMeetingNotificationDTO createDTO)
    {
        return await _notiService.CreateApproveMeetingNoty(createDTO);
    }

    [NonAction]
    [CapSubscribe(WorkerConst.CONFIRM_MET_NOTIFICATION)]
    public async Task<bool> PushConfirmMetNotification(ConfirmMetNotificationDTO createDTO)
    {
        return await _notiService.CreateConfirmMetNoty(createDTO);
    }
}