using Microsoft.AspNetCore.Mvc;
using Monolithic.Models.DTO;
using Monolithic.Services.Interface;

namespace Monolithic.Controllers;

public class AddressController : BaseController
{
    private readonly IAddressService _addressService;

    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpGet("province")]
    public async Task<ActionResult<List<ProvinceDTO>>> GetAllProvince()
    {
        var result = await _addressService.GetAllProvince();
        return Ok(result);
    }

    [HttpGet("/district")]
    public async Task<ActionResult<ProvinceDTO>> GetAllDistrictOfProvince([FromQuery] int provinceId)
    {
        var result = await _addressService.GetAllDistrictOfProvince(provinceId);
        return Ok(result);
    }

    [HttpGet("/ward")]
    public async Task<ActionResult<DistrictDTO>> GetAllWardOfDistrict([FromQuery] int districtId)
    {
        var result = await _addressService.GetAllWardOfDistrict(districtId);
        return Ok(result);
    }

    [HttpGet("/address-string")]
    public async Task<IActionResult> GetAddressStringByWardId([FromQuery] int wardId)
    {
        var result = await _addressService.GetAddress(wardId);
        return Ok(result);   
    }
}