using Monolithic.Models.Common;
using Monolithic.Models.Entities;
namespace Monolithic.Repositories.Interface;

public interface IPostRepository
{
    Task<List<PostEntity>> GetAllPost();
    Task<PostEntity> GetPostById(int id);
    Task<PostEntity> CreatePost(PostEntity postEntity);
}