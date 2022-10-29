using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;
using Monolithic.Models.Entities;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Constants;
using AutoMapper;

namespace Monolithic.Services.Implement;

public class ConfigSettingService : IConfigSettingService
{
    private readonly IConfigSettingRepository _configSettingRepo;
    private readonly IMapper _mapper;
    public ConfigSettingService(IConfigSettingRepository configSettingRepo,
                                IMapper mapper)
    {
        _configSettingRepo = configSettingRepo;
        _mapper = mapper;
    }

    public async Task<List<ConfigSettingDTO>> GetAllConfigSettings()
    {
        var allConfigSettings = await _configSettingRepo.GetAllConfigSettings();
        return allConfigSettings.Select(c => _mapper.Map<ConfigSettingDTO>(c)).ToList();
    }

    public async Task<ConfigSettingDTO> GetByKey(string key)
    {
        var configSetting = await _configSettingRepo.GetByKey(key);
        if (configSetting == null)
            throw new BaseException(HttpCode.NOT_FOUND, "This config setting key is not found");
        return _mapper.Map<ConfigSettingDTO>(configSetting);
    }

    public async Task<double> GetValueByKey(string key)
    {
        var configSetting = await _configSettingRepo.GetByKey(key);
        if (configSetting == null)
            throw new BaseException(HttpCode.NOT_FOUND, "Lost config setting in database");
        return configSetting.Value;
    }

    public async Task<bool> UpdateForKey(string key, ConfigSettingUpdateDTO configSettingUpdateDTO)
    {
        var configSettingUpdate = _mapper.Map<ConfigSettingEntity>(configSettingUpdateDTO);
        return await _configSettingRepo.UpdateForKey(key, configSettingUpdate);
    }
}