using AutoMapper;
using Monolithic.Common;
using Monolithic.Models.DTO;
using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;

namespace Monolithic.Services.Implement;

public class AddressService : IAddressService
{
    private IAddressRepository _addressRepo;
    private IMapper _mapper;

    public AddressService(IAddressRepository addressRepo, IMapper mapper)
    {
        _addressRepo = addressRepo;
        _mapper = mapper;
    }
    public async Task<List<ProvinceDTO>> GetAllProvince()
    {
        var provinceList = await _addressRepo.GetProvince();
        return provinceList.Select(p => _mapper.Map<ProvinceDTO>(p)).ToList();
    }

    public async Task<ProvinceDTO> GetAllDistrictOfProvince(int provinceId)
    {
        var province = await _addressRepo.GetProvince(provinceId);
        return _mapper.Map<ProvinceDTO>(province);
    }

    public async Task<DistrictDTO> GetAllWardOfDistrict(int districtId)
    {
        var district = await _addressRepo.GetDistrict(districtId);
        return _mapper.Map<DistrictDTO>(district);
    }

    public async Task<AddressDTO> GetAddress(int wardId)
    {
        var address = await _addressRepo.GetAddress(wardId);
        var result = _mapper.Map<AddressDTO>(address);
        return result;
    }
}