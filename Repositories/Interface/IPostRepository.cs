using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Models.Entities;
namespace Monolithic.Repositories.Interface;

public interface IPostRepository
{
    Task<List<PostEntity>> GetAllPost();
    Task<PostEntity> GetPostById(int id);
    Task<PostEntity> CreatePost(PostEntity postEntity);
    Task<PostEntity> UpdatePost(int postId, UpdatePostDTO updatePostDTO);
    Task DeletePost(int postId);
}