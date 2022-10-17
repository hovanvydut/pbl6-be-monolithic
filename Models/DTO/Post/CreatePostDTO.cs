namespace Monolithic.Models.DTO;

public class CreatePostDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public float Area { get; set; }
    public string Address { get; set; }
    public int AddressWardId { get; set; }
    public float Price { get; set; }
    public float PrePaidPrice { get; set; }
    public int CategoryId { get; set; }
    public int LimitTenant { get; set; }
    public List<CreateMediaDTO> Medias { get; set; }
    public List<int> Properties { get; set; }
}