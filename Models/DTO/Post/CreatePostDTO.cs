namespace Monolithic.Models.DTO;

public class CreatePostDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public double Area { get; set; }
    public string Address { get; set; }
    public int AddressWardId { get; set; }
    public double Price { get; set; }
    public double PrePaidPrice { get; set; }
    public int UserId { get; set; }
    public int CategoryId { get; set; }
    public int LimitTenant { get; set; }
    public List<CreateMediaDTO> Medias { get; set; }
}