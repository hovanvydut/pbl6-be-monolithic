namespace Monolithic.Models.DTO;

public class BookmarkDTO
{
    public int Id {get;set;}

    public int PostId { get; set; }

    public string PostTitle { get; set; }

    public string PostDescription { get; set; }

    public string PostPrice { get; set; }

    public string PostSlug { get; set; }

    public List<MediaDTO> Medias { get; set; }
}