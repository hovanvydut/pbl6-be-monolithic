using Microsoft.EntityFrameworkCore;
using Monolithic.Models.Context;
using Monolithic.Models.Entities;
using Monolithic.Repositories.Interface;

namespace Monolithic.Repositories.Implement;

public class PropertyRepository : IPropertyRepository
{
    private readonly DataContext _db;

    public PropertyRepository(DataContext db)
    {
        _db = db;
    }

    public async Task<List<PropertyEntity>> GetAllProperties()
    {
        return await _db.Properties.ToListAsync();
    }

    public async Task<List<PropertyGroupEntity>> GetAllPropertiesGroup()
    {
        return await _db.PropertyGroups.Include(p => p.Properties).ToListAsync();
        // return await _db.PropertyGroups.ToListAsync();
    }
}