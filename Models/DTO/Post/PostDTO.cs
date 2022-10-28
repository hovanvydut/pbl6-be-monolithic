namespace Monolithic.Models.DTO;

public class PostDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public float Area { get; set; }
    public float Price { get; set; }
    public float PrePaidPrice { get; set; }
    public string Slug { get; set; }
    public int LimitTenant { get; set; }
    public int NumView { get; set; }
    public string Address { get; set; }
    public FullAddressDTO FullAddress { get; set; }
    public CategoryDTO Category { get; set; }
    public List<PropertyDTO> Properties { get; set; }
    public List<PropertyGroupDTO> PropertyGroup { get; set; }
    public List<MediaDTO> Medias { get; set; }
    public bool isBookmarked { get; set; }
}