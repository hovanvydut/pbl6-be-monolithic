namespace Monolithic.Models.DTO;

public class UserProfilePersonalDTO
{
    public int Id { get; set; }

    public string DisplayName { get; set; }

    public string PhoneNumber { get; set; }

    public string IdentityNumber { get; set; }

    public double CurrentCredit { get; set; }

    public string Address { get; set; }

    public int AddressWardId { get; set; }

    public string AddressWard { get; set; }

    public string AddressDistrict { get; set; }

    public string AddressProvince { get; set; }

    public int UserAccountId { get; set; }

    public string UserAccountEmail { get; set; }
}