using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Models.ReqParams;

namespace Monolithic.Services.Interface;

public interface IPostService
{
    Task<PostDTO> GetPostById(int id, int userId = 0);
    Task<List<PostDTO>> GetRelatedPost(RelatedPostParams relatedPostParams);
    Task<PagedList<PostDTO>> GetWithParamsInSearchAndFilter(int guestId, PostSearchFilterParams postParams);
    Task<PagedList<PostDTO>> GetWithParamsInTableAndList(int hostId, int guestId, PostTableListParams postParams);
    Task CreatePost(int hostId, CreatePostDTO createPostDTO);
    Task UpdatePost(int hostId, int postId, UpdatePostDTO updatePostDTO);
    Task DeletePost(int hostId, int postId);
}