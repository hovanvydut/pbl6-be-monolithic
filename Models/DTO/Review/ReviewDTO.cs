namespace Monolithic.Models.DTO;

public class ReviewDTO
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public UserProfileAnonymousDTO UserInfo { get; set; }
    public string Content { get; set; }
    public int Rating { get; set; }
    public List<MediaDTO> Medias { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}