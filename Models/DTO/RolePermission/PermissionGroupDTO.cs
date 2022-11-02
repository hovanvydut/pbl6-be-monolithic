namespace Monolithic.Models.DTO;

public class PermissionGroupDTO
{
    public string Key { get; set; }

    public List<PermissionDTO> Children { get; set; }
}