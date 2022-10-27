namespace Monolithic.Models.DTO;

public class PermissionGroupDTO
{
    public string Name { get; set; }

    public List<PermissionDTO> Children { get; set; }
}