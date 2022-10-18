namespace Monolithic.Models.DTO;

public class CreatePermissionDTO
{
    public string Key { get; set; }

    public string Description { get; set; }

    public int RoleId { get; set; }
}