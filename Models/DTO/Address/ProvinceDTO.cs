namespace Monolithic.Models.DTO;

public class ProvinceDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<DistrictDTO> AddressDistricts { get; set; }
}