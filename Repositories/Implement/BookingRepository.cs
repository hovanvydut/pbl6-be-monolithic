using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Monolithic.Constants;
using Monolithic.Extensions;
using Monolithic.Models.Common;
using Monolithic.Models.Context;
using Monolithic.Models.DTO;
using Monolithic.Models.Entities;
using Monolithic.Models.ReqParams;
using Monolithic.Repositories.Interface;

namespace Monolithic.Repositories.Implement;

public class BookingRepository : IBookingRepository
{
    private readonly DataContext _db;
    private readonly IMapper _mapper;
    public BookingRepository(DataContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task Approve(int meetingId)
    {
        var meetingEntity = await this.GetMeetingById(meetingId);
        if (meetingEntity == null) {
            throw new BaseException(HttpCode.NOT_FOUND, "Meeting id = " + meetingId + " doesn't found");
        }

        meetingEntity.ApproveTime = new DateTime();
        await _db.SaveChangesAsync();
    }

    public async Task ConfirmMeet(int meetingId)
    {
        var meetingEntity = await this.GetMeetingById(meetingId);
        if (meetingEntity == null) {
            throw new BaseException(HttpCode.NOT_FOUND, "Meeting id = " + meetingId + " doesn't found");
        }

        meetingEntity.Met = true;
        await _db.SaveChangesAsync();
    }

    public async Task<MeetingEntity> CreateMeeting(int userId, CreateMeetingDTO dto)
    {
        MeetingEntity entity = _mapper.Map<MeetingEntity>(dto);
        entity.GuestId = userId;
        // entity.ApproveTime = null;
        await _db.Meetings.AddAsync(entity);
        await _db.SaveChangesAsync();
        Console.WriteLine("Approve Time = " + entity.ApproveTime.ToString());
        return await GetMeetingById(entity.Id);
    }

    public async Task DeleteAllFreeTime(int userId)
    {
        _db.FreeTimes.RemoveRange(_db.FreeTimes.Where(f => f.UserId == userId));
        await _db.SaveChangesAsync();
    }

    public async Task<List<FreeTimeEntity>> GetAllFreeTime(int userId)
    {
        return await _db.FreeTimes.Where(f => f.UserId == userId).ToListAsync();
    }

    public async Task<PagedList<MeetingEntity>> GetAllMeeting(int userId, BookingParams reqParams)
    {
        var meetings = _db.Meetings
                .Include(m => m.GuestAccount.UserProfile)
                .OrderByDescending(c => c.CreatedAt)
                .Where(m => m.Post.HostId == userId);

        if (reqParams.month > 0 && reqParams.month <= 12)
        {
            meetings = meetings.Where(m => m.Time.Month == reqParams.month);
        }

        if (reqParams.year > 0)
        {
            meetings = meetings.Where(m => m.Time.Year == reqParams.year);
        }

        return await meetings.ToPagedList(reqParams.PageNumber, reqParams.PageSize);
    }

    public async Task<PagedList<MeetingEntity>> GetAllMeetingBookedBy(int userId, BookingParams reqParams)
    {
        var meetings = _db.Meetings
                .Include(m => m.GuestAccount.UserProfile)
                .OrderByDescending(c => c.CreatedAt)
                .Where(m => m.GuestId == userId);

        if (reqParams.month > 0 && reqParams.month <= 12)
        {
            meetings = meetings.Where(m => m.Time.Month == reqParams.month);
        }

        if (reqParams.year > 0)
        {
            meetings = meetings.Where(m => m.Time.Year == reqParams.year);
        }

        return await meetings.ToPagedList(reqParams.PageNumber, reqParams.PageSize);
    }

    public async Task<List<MeetingEntity>> GetMeetingBy(int userId, int postId)
    {
        return await _db.Meetings.Where(m => m.PostId == postId && m.GuestId == userId).ToListAsync();
    }

    public async Task<MeetingEntity> GetMeetingById(int meetingId)
    {
        return await _db.Meetings.Include(m => m.Post).Where(m => m.Id == meetingId).FirstOrDefaultAsync();
    }

    public async Task<MeetingEntity> GetMeetingByUserIdAndMeetingId(int userId, int meetingId)
    {
        return await _db.Meetings.Include(m => m.Post)
            .Include(m => m.GuestAccount)
            .Where(m => m.Id == meetingId).FirstOrDefaultAsync();
    }

    public async Task InsertAllFreeTime(int userId, CreateFreeTimeDTO dto)
    {
        foreach (FreeTimeDTO freeTime in dto.Data)
        {
            FreeTimeEntity entity = _mapper.Map<FreeTimeEntity>(freeTime);
            entity.UserId = userId;
            _db.FreeTimes.Add(entity);
        }
        await _db.SaveChangesAsync();
    }
}