using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Models.Entities;
using Monolithic.Models.ReqParams;

namespace Monolithic.Repositories.Interface;

public interface IBookingRepository
{
    Task<List<FreeTimeEntity>> GetAllFreeTime(int userId);
    Task DeleteAllFreeTime(int userId);
    Task InsertAllFreeTime(int userId, CreateFreeTimeDTO dto);
    Task<MeetingEntity> CreateMeeting(int userId, CreateMeetingDTO dto);
    Task<MeetingEntity> GetMeetingById(int meetingId);
    Task<PagedList<MeetingEntity>> GetAllMeetingBookedBy(int userId, BookingParams reqParams);
    Task Approve(int meetingId);
    Task ConfirmMeet(int meetingId);
    Task<PagedList<MeetingEntity>> GetAllMeeting(int userId, BookingParams reqParams);
    Task<List<MeetingEntity>> GetMeetingBy(int userId, int postId);
    Task<MeetingEntity> GetMeetingByUserIdAndMeetingId(int userId, int meetingId);
}