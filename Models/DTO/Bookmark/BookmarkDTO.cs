namespace Monolithic.Models.DTO;

public class BookmarkDTO
{
    public int Id { get; set; }

    public string Title { get; set; }

    public float Price { get; set; }

    public string Slug { get; set; }

    public float Area { get; set; }

    public string Address { get; set; }

    public double AverageRating { get; set; }

    public FullAddressDTO FullAddress { get; set; }

    public CategoryDTO Category { get; set; }

    public List<MediaDTO> Medias { get; set; }
}
