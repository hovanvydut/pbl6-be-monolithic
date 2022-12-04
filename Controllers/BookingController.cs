using static Monolithic.Constants.PermissionPolicy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Constants;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Models.ReqParams;
using Monolithic.Services.Interface;

namespace Monolithic.Controllers;

public class BookingController : BaseController
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        this._bookingService = bookingService;
    }

    [HttpGet("user/{userId}/free-time")]
    [Authorize(Roles = FreeTimePermission.ViewAll)]
    public async Task<BaseResponse<List<FreeTimeDTO>>> GetAllFreeTime(int userId)
    {
        // get tat ca lich ranh cua chu tro
        List<FreeTimeDTO> result = await _bookingService.GetAllFreeTime(userId);
        return new BaseResponse<List<FreeTimeDTO>>(result);
    }

    [HttpPost("free-time")]
    [Authorize(Roles = FreeTimePermission.Create)]
    public async Task<BaseResponse<bool>> CreateFreeTime([FromBody] CreateFreeTimeDTO dto)
    {
        // chu tro tao lich ranh hang tuan
        ReqUser reqUser = HttpContext.Items["reqUser"] as ReqUser;
        await _bookingService.CreateFreeTime(reqUser.Id, dto);
        return new BaseResponse<bool>(true, HttpCode.CREATED);
    }

    [HttpGet("personal")]
    [Authorize(Roles = BookingPermission.ViewAllPersonal)]
    public async Task<BaseResponse<PagedList<MeetingDTO>>> GetAllMeeting([FromQuery] BookingParams reqParams)
    {
        // get all meeting cua chu tro trong thang
        ReqUser reqUser = HttpContext.Items["reqUser"] as ReqUser;
        PagedList<MeetingDTO> result = await _bookingService.GetAllMeeting(reqUser.Id, reqParams);
        return new BaseResponse<PagedList<MeetingDTO>>(result);
    }

    [HttpGet("booked-by-user")]
    [Authorize(Roles = BookingPermission.ViewAllBooked)]
    public async Task<BaseResponse<PagedList<MeetingDTO>>> GetAllMeetingBookedBy([FromQuery] BookingParams reqParams)
    {
        // get all meeting which was booked by user
        ReqUser reqUser = HttpContext.Items["reqUser"] as ReqUser;
        PagedList<MeetingDTO> result = await _bookingService.GetAllMeetingBookedBy(reqUser.Id, reqParams);
        return new BaseResponse<PagedList<MeetingDTO>>(result);
    }

    [HttpGet("booked-by-user/{meetingId}")]
    [Authorize(Roles = BookingPermission.ViewAllBooked)]
    public async Task<BaseResponse<MeetingDTO>> GetMeetingBookedBy(int meetingId)
    {
        // get all meeting which was booked by user
        ReqUser reqUser = HttpContext.Items["reqUser"] as ReqUser;
        MeetingDTO result = await _bookingService.GetMeetingBookedBy(reqUser.Id, meetingId);
        return new BaseResponse<MeetingDTO>(result);
    }

    [HttpPost("")]
    [Authorize(Roles = BookingPermission.CreateMeeting)]
    public async Task<BaseResponse<bool>> CreateMeeting([FromBody] CreateMeetingDTO dto)
    {
        // nguoi thue tro tao meeting
        ReqUser reqUser = HttpContext.Items["reqUser"] as ReqUser;
        await _bookingService.CreateMeeting(reqUser.Id, dto);
        return new BaseResponse<bool>(true, HttpCode.CREATED);
    }

    [HttpPut("{meetingId}/approve")]
    [Authorize(Roles = BookingPermission.ApproveMeeting)]
    public async Task<BaseResponse<bool>> ApproveMeeting(int meetingId)
    {
        // chu tro confirm xac nhan gap meeting do khong
        ReqUser reqUser = HttpContext.Items["reqUser"] as ReqUser;
        await _bookingService.ApproveMeeting(reqUser.Id, meetingId);
        return new BaseResponse<bool>(true, HttpCode.OK);
    }

    [HttpPut("{meetingId}/confirm-meet")]
    [Authorize(Roles = BookingPermission.ConfirmMet)]
    public async Task<BaseResponse<bool>> ConfirmMeeting(int meetingId)
    {
        ReqUser reqUser = HttpContext.Items["reqUser"] as ReqUser;
        await _bookingService.ConfirmMeeting(reqUser.Id, meetingId);
        return new BaseResponse<bool>(true, HttpCode.OK);
    }
}