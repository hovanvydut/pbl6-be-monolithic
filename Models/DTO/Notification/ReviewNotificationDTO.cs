namespace Monolithic.Models.DTO;

public class ReviewNotificationDTO
{
    public int OriginUserId { get; set; }
    public int PostId { get; set; }
    public int ReviewId { get; set; }
    public string ReviewContent { get; set; }
    public int ReviewRating { get; set; }
}