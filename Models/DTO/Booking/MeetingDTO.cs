namespace Monolithic.Models.DTO;

public class MeetingDTO
{
    public int Id { get; set; }
    public GuestMeetingDTO GuestInfo { get; set; }
    public DateTime Time { get; set; }
    public DateTime? ApproveTime { get; set; }
    public bool Met { get; set; }
}