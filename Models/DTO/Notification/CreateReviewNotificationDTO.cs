namespace Monolithic.Models.DTO;

public class CreateReviewNotificationDTO
{
    public int OriginUserId { get; set; }
    public string Content { get; set; }
    public int PostId { get; set; }
    public int ReviewId { get; set; }
}