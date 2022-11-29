namespace Monolithic.Models.DTO;

public class CreateBookingNotificationDTO
{
    public int OriginUserId { get; set; }
    public int PostId { get; set; }
    public int BookingId { get; set; }
}