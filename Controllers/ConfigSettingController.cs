using static Monolithic.Constants.PermissionPolicy;
using Microsoft.AspNetCore.Authorization;
using Monolithic.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Constants;

namespace Monolithic.Controllers;

public class ConfigSettingController : BaseController
{
    private readonly IConfigSettingService _configSettingService;

    public ConfigSettingController(IConfigSettingService configSettingService)
    {
        _configSettingService = configSettingService;
    }

    [HttpGet]
    public async Task<BaseResponse<List<ConfigSettingDTO>>> GetAllConfigSettings()
    {
        var settings = await _configSettingService.GetAllConfigSettings();
        return new BaseResponse<List<ConfigSettingDTO>>(settings);
    }

    [HttpGet("{key}")]
    public async Task<BaseResponse<ConfigSettingDTO>> GetConfigSettingByKey(string key)
    {
        var setting = await _configSettingService.GetByKey(key);
        return new BaseResponse<ConfigSettingDTO>(setting);
    }

    [HttpPut("{key}")]
    public async Task<BaseResponse<bool>> UpdateConfigSetting(string key, ConfigSettingUpdateDTO configSettingUpdateDTO)
    {
        if (ModelState.IsValid)
        {
            var settingUpdated = await _configSettingService.UpdateForKey(key, configSettingUpdateDTO);
            if (settingUpdated)
                return new BaseResponse<bool>(settingUpdated, HttpCode.NO_CONTENT);
            else
                return new BaseResponse<bool>(settingUpdated, HttpCode.BAD_REQUEST, "", false);
        }
        return new BaseResponse<bool>(false, HttpCode.BAD_REQUEST, "", false);
    }
}