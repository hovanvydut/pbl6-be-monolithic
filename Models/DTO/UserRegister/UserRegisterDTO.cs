namespace Monolithic.Models.DTO;

public class UserRegisterDTO
{
    public string Email { get; set; }

    public string Password { get; set; }

    public string DisplayName { get; set; }

    public string PhoneNumber { get; set; }

    public string IdentityNumber { get; set; }

    public string Address { get; set; }

    public int AddressWardId { get; set; }

    public int RoleId { get; set; }
}