namespace Monolithic.Models.DTO;
public class NotificationDTO
{
    public int Id { get; set; }
    public int OriginUserId { get; set; }
    public string OriginUserEmail { get; set; }
    public string OriginUserName { get; set; }
    public string OriginUserAvatar { get; set; }
    public string Code { get; set; }
    public bool HasRead { get; set; }
    public string ExtraData { get; set; }
    public DateTime CreatedAt { get; set; }
}