namespace Monolithic.Models.DTO;

public class FullAddressDTO
{
    public AddressDTO ward { get; set; }
    public AddressDTO district { get; set; }
    public AddressDTO province { get; set; }
}