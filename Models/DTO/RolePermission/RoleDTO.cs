namespace Monolithic.Models.DTO;

public class RoleDTO
{
    public string Name { get; set; }

    public string Description { get; set; }

    public List<PermissionDTO> Permissions { get; set; }
}