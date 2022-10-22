using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Models.ReqParams;

namespace Monolithic.Services.Interface;

public interface IPostService
{
    Task<PostDTO> GetPostById(int id);
    Task<PagedList<PostDTO>> GetPostWithParams(PostParams postParams);
    Task<List<PostDTO>> GetAllPost();
    Task CreatePost(CreatePostDTO createPostDTO);
    Task UpdatePost(int postId, UpdatePostDTO updatePostDTO);
    Task DeletePost(int postId);
}