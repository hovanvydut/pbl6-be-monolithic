namespace Monolithic.Models.DTO;

public class UserRegisterDTO
{
    public string Email { get; set; }

    public string Password { get; set; }

    public int RoleId { get; set; }
}