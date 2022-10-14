namespace Monolithic.Models.DTO;

public class PropertyGroupDTO
{
    public int Id { get; set; }
    public string DisplayName { get; set; }
    public List<PropertyDTO> Properties { get; set; }
}