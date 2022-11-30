using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Models.Entities;
using Monolithic.Models.ReqParams;

namespace Monolithic.Repositories.Interface;

public interface IPostRepository
{
    Task<List<PostEntity>> GetRelatedPost(RelatedPostParams relatedPostParams);
    Task<PagedList<PostEntity>> GetWithParamsInSearchAndFilter(PostSearchFilterParams postParams, IEnumerable<int> priorityIds);
    Task<PagedList<PostEntity>> GetWithParamsInTableAndList(int hostId, PostTableListParams postParams, IEnumerable<int> priorityIds);
    Task<PostEntity> GetPostById(int id);
    Task<PostEntity> CreatePost(PostEntity postEntity);
    Task<PostEntity> UpdatePost(int hostId, int postId, UpdatePostDTO updatePostDTO);
    Task<bool> DeletePost(int hostId, int postId);
}