using Monolithic.Models.ReqParams;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;

namespace Monolithic.Services.Interface;

public interface INotificationService
{
    Task<PagedList<NotificationDTO>> GetNotifications(int userId, NotificationParams notificationParams);

    Task<CountUnreadNotificationDTO> CountUnreadNotification(int userId);

    Task<bool> SetNotyHasRead(int userId, int notyId);

    Task<bool> SetAllNotyHasRead(int userId);

    Task<bool> CreateReviewOnPostNoty(ReviewNotificationDTO createDTO);

    Task<bool> CreateBookingOnPostNoty(BookingNotificationDTO createDTO);

    Task<bool> CreateApproveMeetingNoty(ApproveMeetingNotificationDTO createDTO);

    Task<bool> CreateConfirmMetNoty(ConfirmMetNotificationDTO createDTO);
}