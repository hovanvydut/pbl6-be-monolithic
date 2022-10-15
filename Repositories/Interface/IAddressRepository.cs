using Monolithic.Models.Entities;

namespace Monolithic.Repositories.Interface;

public interface IAddressRepository
{
    Task<List<AddressProvinceEntity>> GetProvince();
    Task<AddressProvinceEntity> GetProvince(int id);
    Task<List<AddressDistrictEntity>> GetDistrict();
    Task<AddressDistrictEntity> GetDistrict(int id);
    Task<AddressWardEntity> GetAddress(int wardId);
}