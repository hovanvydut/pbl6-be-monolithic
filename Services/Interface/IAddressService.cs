using Monolithic.Models.DTO;

namespace Monolithic.Services.Interface;

public interface IAddressService
{
    Task<List<ProvinceDTO>> GetAllProvince();
    Task<ProvinceDTO> GetAllDistrictOfProvince(int provinceId);
    Task<DistrictDTO> GetAllWardOfDistrict(int districtId);
    Task<AddressDTO> GetAddress(int wardId);
}