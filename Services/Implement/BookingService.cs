using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Monolithic.Constants;
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

    public BookingService(IUserService userService, IBookingRepository bookingRepo, DataContext db,
                        IPostRepository postRepo, IMapper mapper)
    {
        this._userService = userService;
        this._bookingRepo = bookingRepo;
        this._db = db;
        this._postRepo = postRepo;
        this._mapper = mapper;
    }

    public async Task ApproveMeeting(int userId, int meetingId)
    {
        using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
        {
            try
            {
                MeetingEntity meetingEntity = await _bookingRepo.GetMeetingById(meetingId);
                if (meetingEntity == null) {
                    throw new BaseException(HttpCode.NOT_FOUND, "Meeting id = " + meetingId + " doesn't found");
                }

                if (meetingEntity.Post.HostId != userId)
                {
                    throw new BaseException(HttpCode.BAD_REQUEST, "You can't approve this meeting");
                }

                if (meetingEntity.ApproveTime != null) {
                    throw new BaseException(HttpCode.BAD_REQUEST, "This meeting is approved");
                }

                await _bookingRepo.Approve(meetingId);

                transaction.Commit();
            }
            catch (BaseException ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }
    }

    public async Task ConfirmMeeting(int userId, int meetingId)
    {
        using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
        {
            try
            {
                MeetingEntity meetingEntity = await _bookingRepo.GetMeetingById(meetingId);
                if (meetingEntity == null) {
                    throw new BaseException(HttpCode.NOT_FOUND, "Meeting id = " + meetingId + " doesn't found");
                }

                if (meetingEntity.Post.HostId != userId)
                {
                    throw new BaseException(HttpCode.BAD_REQUEST, "You can't approve this meeting");
                }

                if (meetingEntity.Met == true) {
                    throw new BaseException(HttpCode.BAD_REQUEST, "This meeting is confirmed");
                }

                await _bookingRepo.ConfirmMeet(meetingId);

                transaction.Commit();
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
                    throw new BaseException(HttpCode.NOT_FOUND, "Post id = " + postEntity.Id + " doesn't found");

                if (postEntity.HostId == userId)
                {
                    throw new BaseException(HttpCode.BAD_REQUEST, "You doesn't allow to create meeting for this post");
                }

                await _bookingRepo.CreateMeeting(userId, dto);

                transaction.Commit();
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
}