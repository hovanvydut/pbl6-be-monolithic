using Microsoft.AspNetCore.Mvc;
using Monolithic.Models.DTO;
using Monolithic.Services.Interface;
using Monolithic.Models.Common;
using Serilog;

namespace Monolithic.Controllers;

public class AddressController : BaseController
{
    private readonly IAddressService _addressService;
    private readonly ILogger<AddressController> _log;

    public AddressController(IAddressService addressService, ILogger<AddressController> log)
    {
        _addressService = addressService;
        _log = log;
    }

    [HttpGet("test-log")]
    public async Task TestLog()
    {
        _log.LogInformation("The global logger has been configured11111");
        _log.LogInformation("2222No one listens to me!");
        _log.LogInformation("33333333No one listens to me!");
        try {
            throw new Exception("oke");
        } catch (Exception e) {
            _log.LogError(e, "Error nek");
        }

    }

    [HttpGet("province")]
    public async Task<BaseResponse<List<ProvinceDTO>>> GetAllProvince()
    {
        var result = await _addressService.GetAllProvince();
        return new BaseResponse<List<ProvinceDTO>>(result);
    }

    [HttpGet("district")]
    public async Task<BaseResponse<ProvinceDTO>> GetAllDistrictOfProvince([FromQuery] int provinceId)
    {
        var result = await _addressService.GetAllDistrictOfProvince(provinceId);
        return new BaseResponse<ProvinceDTO>(result);
    }

    [HttpGet("ward")]
    public async Task<BaseResponse<DistrictDTO>> GetAllWardOfDistrict([FromQuery] int districtId)
    {
        var result = await _addressService.GetAllWardOfDistrict(districtId);
        return new BaseResponse<DistrictDTO>(result);
    }

    [HttpGet("full-address")]
    public async Task<BaseResponse<FullAddressDTO>> GetAddressStringByWardId([FromQuery] int wardId)
    {
        var result = await _addressService.GetAddress(wardId);
        return new BaseResponse<FullAddressDTO>(result);
    }
}