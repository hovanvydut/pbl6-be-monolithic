using Monolithic.Models.DTO;

namespace Monolithic.Helpers;

public interface INotificationHelper
{
    IHttpHelper<ReviewNotificationDTO> Review { get; }
    IHttpHelper<BookingNotificationDTO> Booking { get; }
    IHttpHelper<ApproveMeetingNotificationDTO> ApproveMeeting { get; }
    IHttpHelper<ConfirmMetNotificationDTO> ConfirmMet { get; }
}