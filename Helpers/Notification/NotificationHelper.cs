using Monolithic.Models.DTO;

namespace Monolithic.Helpers;

public class NotificationHelper : INotificationHelper
{
    private readonly IHttpClientFactory _httpClientFactory;
    public NotificationHelper(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;

        Review = new HttpHelper<ReviewNotificationDTO>(_httpClientFactory);
        Booking = new HttpHelper<BookingNotificationDTO>(_httpClientFactory);
        ApproveMeeting = new HttpHelper<ApproveMeetingNotificationDTO>(_httpClientFactory);
        ConfirmMet = new HttpHelper<ConfirmMetNotificationDTO>(_httpClientFactory);
    }

    public IHttpHelper<ReviewNotificationDTO> Review { get; private set; }

    public IHttpHelper<BookingNotificationDTO> Booking { get; private set; }

    public IHttpHelper<ApproveMeetingNotificationDTO> ApproveMeeting { get; private set; }

    public IHttpHelper<ConfirmMetNotificationDTO> ConfirmMet { get; private set; }
}