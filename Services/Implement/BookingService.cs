using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Monolithic.Constants;
using Monolithic.Extensions;
using Monolithic.Models.Common;
using Monolithic.Models.Context;
using Monolithic.Models.DTO;
using Monolithic.Models.Entities;
using Monolithic.Models.ReqParams;
using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;

namespace Monolithic.Services.Implement;

public class BookingService : IBookingService
{
    private readonly DataContext _db;
    private readonly IUserService _userService;
    private readonly IBookingRepository _bookingRepo;
    private readonly IPostRepository _postRepo;
    private readonly IMapper _mapper;
    private readonly IStatisticService _statisticService;
    private readonly INotificationService _notyService;

    public BookingService(IUserService userService, IBookingRepository bookingRepo,
                         IPostRepository postRepo, IMapper mapper, DataContext db,
                         IStatisticService statisticService, INotificationService notyService)
    {
        _userService = userService;
        _bookingRepo = bookingRepo;
        _db = db;
        _postRepo = postRepo;
        _mapper = mapper;
        _statisticService = statisticService;
        _notyService = notyService;
    }

    public async Task ApproveMeeting(int userId, int meetingId)
    {
        using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
        {
            try
            {
                MeetingEntity meetingEntity = await _bookingRepo.GetMeetingById(meetingId);
                if (meetingEntity == null)
                {
                    string msgErr = "Meeting id = " + meetingId + " doesn't found";
                    throw new BaseException(HttpCode.NOT_FOUND, msgErr);
                }

                if (meetingEntity.Post.HostId != userId)
                {
                    string msgErr = "You can't approve this meeting";
                    throw new BaseException(HttpCode.BAD_REQUEST, msgErr);
                }

                if (meetingEntity.ApproveTime != null)
                {
                    string msgErr = "This meeting is approved";
                    throw new BaseException(HttpCode.BAD_REQUEST, msgErr);
                }

                await _bookingRepo.Approve(meetingId);

                transaction.Commit();

                await _notyService.CreateApproveMeetingNoty(new ApproveMeetingNotificationDTO()
                {
                    TargetUserId = meetingEntity.GuestId,
                    PostId = meetingEntity.PostId,
                    BookingId = meetingEntity.Id,
                });
            }
            catch (BaseException ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }
    }

    public async Task<bool> CheckMetBooking(int userId, int postId)
    {
        try
        {
            // check userId exists
            await _userService.GetUserProfilePersonal(userId);

            List<MeetingEntity> listMeetings = await _bookingRepo.GetMeetingBy(userId, postId);
            if (listMeetings.Count <= 0) return false;

            foreach (MeetingEntity meeting in listMeetings)
            {
                bool hasMet = meeting.Met;
                bool less15Days = (int)DateTime.Now.GetLocalTime().Subtract(meeting.Time).TotalDays <= 15;
                if (hasMet && less15Days)
                {
                    return true;
                }
            }

            return false;
        }
        catch (BaseException)
        {
            return false;
        }
    }

    public async Task ConfirmMeeting(int userId, int meetingId)
    {
        using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
        {
            try
            {
                MeetingEntity meetingEntity = await _bookingRepo.GetMeetingById(meetingId);
                if (meetingEntity == null)
                {
                    string msgErr = "Meeting id = " + meetingId + " doesn't found";
                    throw new BaseException(HttpCode.NOT_FOUND, msgErr);
                }

                if (meetingEntity.Post.HostId != userId)
                {
                    string msgErr = "You can't approve this meeting";
                    throw new BaseException(HttpCode.BAD_REQUEST, msgErr);
                }

                if (meetingEntity.Met == true)
                {
                    string msgErr = "This meeting is confirmed";
                    throw new BaseException(HttpCode.BAD_REQUEST, msgErr);
                }

                await _bookingRepo.ConfirmMeet(meetingId);
                await _statisticService.SaveGuestMetMotelStatistic(meetingEntity.PostId);

                transaction.Commit();

                await _notyService.CreateConfirmMetNoty(new ConfirmMetNotificationDTO()
                {
                    TargetUserId = meetingEntity.GuestId,
                    PostId = meetingEntity.PostId,
                    BookingId = meetingEntity.Id,
                });
            }
            catch (BaseException ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }
    }

    public async Task CreateFreeTime(int userId, CreateFreeTimeDTO dto)
    {
        using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
        {
            try
            {
                // check userId exists
                await _userService.GetUserProfilePersonal(userId);

                // delete all previous free time
                await _bookingRepo.DeleteAllFreeTime(userId);

                await _bookingRepo.InsertAllFreeTime(userId, dto);

                transaction.Commit();
            }
            catch (BaseException ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }
    }

    public async Task CreateMeeting(int userId, CreateMeetingDTO dto)
    {
        using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
        {
            try
            {
                PostEntity postEntity = await _postRepo.GetPostById(dto.PostId);
                if (postEntity == null)
                {
                    string msgErr = "Post id = " + postEntity.Id + " doesn't found";
                    throw new BaseException(HttpCode.NOT_FOUND, msgErr);
                }

                if (postEntity.HostId == userId)
                {
                    string msgErr = "You doesn't allow to create meeting for this post";
                    throw new BaseException(HttpCode.BAD_REQUEST, msgErr);
                }

                MeetingEntity savedEntity = await _bookingRepo.CreateMeeting(userId, dto);

                // Save statistic
                await _statisticService.SaveBookingStatistic(dto.PostId);

                transaction.Commit();

                // Notification
                await _notyService.CreateBookingOnPostNoty(new BookingNotificationDTO()
                {
                    OriginUserId = userId,
                    PostId = dto.PostId,
                    BookingId = savedEntity.Id,
                    BookingTime = savedEntity.Time,
                });
            }
            catch (BaseException ex)
            {
                transaction.Rollback();
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }
    }

    public async Task<List<FreeTimeDTO>> GetAllFreeTime(int userId)
    {
        List<FreeTimeEntity> entities = await _bookingRepo.GetAllFreeTime(userId);
        return entities.Select(e => _mapper.Map<FreeTimeDTO>(e)).ToList();
    }

    public async Task<PagedList<MeetingDTO>> GetAllMeeting(int userId, BookingParams reqParams)
    {
        PagedList<MeetingEntity> meetingEntities = await _bookingRepo.GetAllMeeting(userId, reqParams);
        List<MeetingDTO> meetingDTOList = meetingEntities.Records.Select(p => _mapper.Map<MeetingDTO>(p)).ToList();
        return new PagedList<MeetingDTO>(meetingDTOList, meetingEntities.TotalRecords);
    }

    public async Task<PagedList<MeetingDTO>> GetAllMeetingBookedBy(int userId, BookingParams reqParams)
    {
        PagedList<MeetingEntity> meetingEntities = await _bookingRepo.GetAllMeetingBookedBy(userId, reqParams);
        List<MeetingDTO> meetingDTOList = meetingEntities.Records.Select(p => _mapper.Map<MeetingDTO>(p)).ToList();
        return new PagedList<MeetingDTO>(meetingDTOList, meetingEntities.TotalRecords);
    }

    public async Task<MeetingDTO> GetMeetingBookedBy(int userId, int meetingId)
    {
        var entity = await _bookingRepo.GetMeetingByUserIdAndMeetingId(userId, meetingId);
        return _mapper.Map<MeetingDTO>(entity);
    }
}