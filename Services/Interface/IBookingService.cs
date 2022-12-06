using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Models.ReqParams;

namespace Monolithic.Services.Interface;

public interface IBookingService
{
    Task<List<FreeTimeDTO>> GetAllFreeTime(int userId);
    Task<bool> CheckMetBooking(int userId, int postId);
    Task CreateFreeTime(int userId, CreateFreeTimeDTO dto);
    Task<PagedList<MeetingDTO>> GetAllMeeting(int userId, BookingParams reqParams);
    Task CreateMeeting(int userId, CreateMeetingDTO dto);
    Task ApproveMeeting(int userId, int meetingId);
    Task ConfirmMeeting(int userId, int meetingId);
    Task<PagedList<MeetingDTO>> GetAllMeetingBookedBy(int userId, BookingParams reqParams);
    Task<MeetingDTO> GetMeetingBookedBy(int userId, int meetingId);
}