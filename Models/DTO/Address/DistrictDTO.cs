namespace Monolithic.Models.DTO;

public class DistrictDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<WardDTO> AddressWards { get; set; }
}