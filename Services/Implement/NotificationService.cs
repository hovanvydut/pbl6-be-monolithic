using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;
using Monolithic.Models.Entities;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Constants;
using Monolithic.Helpers;
using DotNetCore.CAP;
using AutoMapper;

namespace Monolithic.Services.Implement;

public class NotificationService : INotificationService
{
    private readonly IPostRepository _postRepo;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly INotificationHelper _notificationHelper;
    private readonly ICapPublisher _capBus;

    public NotificationService(IPostRepository postRepo,
                               IConfiguration configuration,
                               IMapper mapper,
                               INotificationHelper notificationHelper,
                               ICapPublisher capBus)
    {
        _postRepo = postRepo;
        _configuration = configuration;
        _mapper = mapper;
        _notificationHelper = notificationHelper;
        _capBus = capBus;
    }

    // PUSH
    public async Task<T> PushNotification<T>(T createDTO)
    {
        try
        {
            string name = "";
            switch (createDTO)
            {
                case ReviewNotificationDTO _:
                    name = WorkerConst.REVIEW_NOTIFICATION;
                    break;
                case BookingNotificationDTO _:
                    name = WorkerConst.BOOKING_NOTIFICATION;
                    break;
                case ApproveMeetingNotificationDTO _:
                    name = WorkerConst.APPROVE_MEETING_NOTIFICATION;
                    break;
                case ConfirmMetNotificationDTO _:
                    name = WorkerConst.CONFIRM_MET_NOTIFICATION;
                    break;
                default:
                    break;
            }
            await _capBus.PublishAsync(name, createDTO);
            return default(T);
        }
        catch
        {
            return default(T);
        }
    }

    // HANDLE
    public async Task<bool> CreateReviewOnPostNoty(ReviewNotificationDTO createDTO)
    {
        PostEntity post = await _postRepo.GetPostById(createDTO.PostId);
        if (post == null)
            throw new BaseException(HttpCode.BAD_REQUEST, "Invalid review on post");

        createDTO.PostTitle = post.Title;
        createDTO.HostId = post.HostId;

        var notiService = _configuration["NotificationService"];
        string notiUrl = $"{notiService}/api/notification/push/review";
        return await _notificationHelper.Review.CreateAsync(notiUrl, createDTO);
    }

    public async Task<bool> CreateBookingOnPostNoty(BookingNotificationDTO createDTO)
    {
        PostEntity post = await _postRepo.GetPostById(createDTO.PostId);
        if (post == null)
            throw new BaseException(HttpCode.BAD_REQUEST, "Invalid booking on post");

        createDTO.PostTitle = post.Title;
        createDTO.HostId = post.HostId;

        var notiService = _configuration["NotificationService"];
        string notiUrl = $"{notiService}/api/notification/push/booking";
        return await _notificationHelper.Booking.CreateAsync(notiUrl, createDTO);
    }

    public async Task<bool> CreateApproveMeetingNoty(ApproveMeetingNotificationDTO createDTO)
    {
        PostEntity post = await _postRepo.GetPostById(createDTO.PostId);
        if (post == null)
            throw new BaseException(HttpCode.BAD_REQUEST, "Invalid meeting on post");

        createDTO.PostTitle = post.Title;
        createDTO.HostId = post.HostId;

        var notiService = _configuration["NotificationService"];
        string notiUrl = $"{notiService}/api/notification/push/approve-meeting";
        return await _notificationHelper.ApproveMeeting.CreateAsync(notiUrl, createDTO);
    }

    public async Task<bool> CreateConfirmMetNoty(ConfirmMetNotificationDTO createDTO)
    {
        PostEntity post = await _postRepo.GetPostById(createDTO.PostId);
        if (post == null)
            throw new BaseException(HttpCode.BAD_REQUEST, "Invalid meeting on post");

        createDTO.PostTitle = post.Title;
        createDTO.HostId = post.HostId;

        var notiService = _configuration["NotificationService"];
        string notiUrl = $"{notiService}/api/notification/push/confirm-met";
        return await _notificationHelper.ConfirmMet.CreateAsync(notiUrl, createDTO);
    }
}