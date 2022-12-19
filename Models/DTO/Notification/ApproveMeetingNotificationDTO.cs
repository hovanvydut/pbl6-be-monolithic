namespace Monolithic.Models.DTO;

public class ApproveMeetingNotificationDTO
{
    public int TargetUserId { get; set; }
    public int PostId { get; set; }
    public string PostTitle { get; set; }
    public int HostId { get; set; }
    public int BookingId { get; set; }
}