using Monolithic.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Monolithic.Models.Entities;
using Monolithic.Models.Context;

namespace Monolithic.Repositories.Implement;

public class ConfigSettingRepository : IConfigSettingRepository
{
    private readonly DataContext _db;
    public ConfigSettingRepository(DataContext db)
    {
        _db = db;
    }

    public async Task<List<ConfigSettingEntity>> GetAllConfigSettings()
    {
        return await _db.ConfigSettings.ToListAsync();
    }

    public async Task<ConfigSettingEntity> GetByKey(string key)
    {
        var config = await _db.ConfigSettings.FirstOrDefaultAsync(c => c.Key == key);
        if (config == null) return null;
        _db.Entry(config).State = EntityState.Detached;
        return config;
    }

    public async Task<bool> UpdateForKey(string key, ConfigSettingEntity configSettingEntity)
    {
        var configDB = await GetByKey(key);
        if (configDB == null) return false;
        configSettingEntity.Id = configDB.Id;
        configSettingEntity.Key = configDB.Key;
        _db.ConfigSettings.Update(configSettingEntity);
        return await _db.SaveChangesAsync() >= 0;
    }
}