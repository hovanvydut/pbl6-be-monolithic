namespace Monolithic.Models.DTO;

public class UserRecoverPasswordDTO
{
    public int UserId { get; set; }

    public string NewPassword { get; set; }
}