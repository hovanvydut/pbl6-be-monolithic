using Monolithic.Models.Entities;

namespace Monolithic.Repositories.Interface;

public interface IConfigSettingRepository
{
    Task<List<ConfigSettingEntity>> GetAllConfigSettings();

    Task<ConfigSettingEntity> GetByKey(string key);

    Task<bool> UpdateForKey(string key, ConfigSettingEntity configSettingEntity);
}