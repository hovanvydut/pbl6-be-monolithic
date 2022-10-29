using Monolithic.Models.DTO;

namespace Monolithic.Services.Interface;

public interface IConfigSettingService
{
    Task<List<ConfigSettingDTO>> GetAllConfigSettings();

    Task<ConfigSettingDTO> GetByKey(string key);

    Task<double> GetValueByKey(string key);

    Task<bool> UpdateForKey(string key, ConfigSettingUpdateDTO configSettingUpdateDTO);
}