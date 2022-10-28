using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Models.ReqParams;

namespace Monolithic.Services.Interface;

public interface IPostService
{
    Task<PostDTO> GetPostById(int id);
    Task<List<PostDTO>> GetRelatedPost(RelatedPostParams relatedPostParams);
    Task<PagedList<PostDTO>> GetPostWithParams(int hostId, PostParams postParams);
    Task<List<PostDTO>> GetAllPost();
    Task CreatePost(int hostId, CreatePostDTO createPostDTO);
    Task UpdatePost(int hostId, int postId, UpdatePostDTO updatePostDTO);
    Task DeletePost(int hostId, int postId);
}