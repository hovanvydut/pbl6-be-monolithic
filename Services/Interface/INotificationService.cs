using Monolithic.Models.DTO;

namespace Monolithic.Services.Interface;

public interface INotificationService
{
    Task<T> PushNotification<T>(T createDTO);

    Task<bool> CreateReviewOnPostNoty(ReviewNotificationDTO createDTO);

    Task<bool> CreateBookingOnPostNoty(BookingNotificationDTO createDTO);

    Task<bool> CreateApproveMeetingNoty(ApproveMeetingNotificationDTO createDTO);

    Task<bool> CreateConfirmMetNoty(ConfirmMetNotificationDTO createDTO);
}