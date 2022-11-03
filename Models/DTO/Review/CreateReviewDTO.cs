namespace Monolithic.Models.DTO;

public class CreateReviewDTO
{
    public string Content { get; set; }
    public List<CreateMediaDTO> Medias { get; set; }
    public int Rating { get; set; }
}