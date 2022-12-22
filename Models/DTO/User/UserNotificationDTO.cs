namespace Monolithic.Models.DTO;

public class UserNotificationDTO
{
    public int OriginUserId { get; set; }
    public string OriginUserEmail { get; set; }
    public string OriginUserName { get; set; }
    public string OriginUserAvatar { get; set; }
}