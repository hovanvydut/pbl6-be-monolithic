using Microsoft.AspNetCore.Authorization;
using Monolithic.Services.Interface;
using Monolithic.Models.ReqParams;
using Monolithic.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Models.DTO;
using Monolithic.Constants;

namespace Monolithic.Controllers;

public class NotificationController : BaseController
{
    private INotificationService _notyService;
    public NotificationController(INotificationService notyService)
    {
        _notyService = notyService;
    }

    [HttpGet]
    [Authorize]
    public async Task<BaseResponse<PagedList<NotificationDTO>>> GetPersonalNotfication(
                        [FromQuery] NotificationParams notificationParams)
    {
        ReqUser reqUser = HttpContext.Items["reqUser"] as ReqUser;
        var notifications = await _notyService.GetNotifications(reqUser.Id, notificationParams);
        return new BaseResponse<PagedList<NotificationDTO>>(notifications);
    }

    [HttpPut("has-read/{id}")]
    [Authorize]
    public async Task<BaseResponse<bool>> SetNotificationHasRead(int id)
    {
        ReqUser reqUser = HttpContext.Items["reqUser"] as ReqUser;
        var notyUpdated = await _notyService.SetNotyHasRead(reqUser.Id, id);
        if (notyUpdated)
            return new BaseResponse<bool>(notyUpdated, HttpCode.NO_CONTENT);
        else
            return new BaseResponse<bool>(notyUpdated, HttpCode.BAD_REQUEST, "", false);
    }
}