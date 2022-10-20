namespace Monolithic.Models.DTO;

public class UserProfileUpdateDTO
{
    public string DisplayName { get; set; }

    public string PhoneNumber { get; set; }

    public string Avatar { get; set; }

    public string Address { get; set; }

    public int AddressWardId { get; set; }
}