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

    public async Task<PropertyEntity> GetPropertyById(int id)
    {
        return await _db.Properties.FindAsync(id);
    }

    public async Task<List<PropertyEntity>> GetAllProperties()
    {
        return await _db.Properties.ToListAsync();
    }

    public async Task<List<PropertyGroupEntity>> GetAllPropertiesGroup()
    {
        return await _db.PropertyGroups.Include(p => p.Properties).ToListAsync();
    }

    public async Task<PropertyEntity> CreateProperty(PropertyEntity propertyEntity)
    {
        await _db.Properties.AddAsync(propertyEntity);
        await _db.SaveChangesAsync();
        return propertyEntity;
    }

    public async Task<List<PropertyEntity>> CreateProperty(List<PropertyEntity> propertyEntities)
    {
        List<PropertyEntity> result = new List<PropertyEntity>();
        foreach (var property in propertyEntities)
        {
            result.Add(await CreateProperty(property));
        }
        return result;
    }

    public async Task<PostPropertyEntity> CreatePostProperty(PostPropertyEntity postPropertyEntity)
    {
        await _db.PostProperties.AddAsync(postPropertyEntity);
        await _db.SaveChangesAsync();
        return postPropertyEntity;
    }

    public async Task<List<PostPropertyEntity>> CreatePostProperty(List<PostPropertyEntity> postPropertyEntities)
    {
        List<PostPropertyEntity> result = new List<PostPropertyEntity>();
        foreach (var postPropertyEntity in postPropertyEntities)
        {
            result.Add(await CreatePostProperty(postPropertyEntity));
        }
        return result;
    }
}