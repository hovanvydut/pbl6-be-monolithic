using Monolithic.Models.Entities;

namespace Monolithic.Repositories.Interface;

public interface IPropertyRepository
{
    Task<PropertyEntity> GetPropertyById(int id);
    Task<List<PropertyEntity>> GetAllProperties();
    Task<List<PropertyGroupEntity>> GetAllPropertiesGroup();
    Task<PropertyEntity> CreateProperty(PropertyEntity propertyEntity);
    Task<List<PropertyEntity>> CreateProperty(List<PropertyEntity> propertyEntities);
    Task<PostPropertyEntity> CreatePostProperty(PostPropertyEntity postPropertyEntity);
    Task<List<PostPropertyEntity>> CreatePostProperty(List<PostPropertyEntity> postPropertyEntities);
    Task DeleteAllPropertyOfPost(int postId);
}