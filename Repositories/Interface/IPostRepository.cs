using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Models.Entities;
using Monolithic.Models.ReqParams;

namespace Monolithic.Repositories.Interface;

public interface IPostRepository
{
    Task<List<PostEntity>> GetAllPost();
    Task<PagedList<PostEntity>> GetPostWithParams(int hostId, PostParams postParams);
    Task<PostEntity> GetPostById(int id);
    Task<PostEntity> CreatePost(PostEntity postEntity);
    Task<PostEntity> UpdatePost(int hostId, int postId, UpdatePostDTO updatePostDTO);
    Task<bool> DeletePost(int hostId, int postId);
}