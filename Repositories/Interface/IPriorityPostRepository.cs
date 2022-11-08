using Monolithic.Models.ReqParams;
using Monolithic.Models.Entities;
using Monolithic.Models.Common;

namespace Monolithic.Repositories.Interface;

public interface IPriorityPostRepository
{
    Task<List<PriorityPostEntity>> GetListDuplicateTime(PriorityPostParams priorityPostParams);

    Task<PagedList<PriorityPostEntity>> GetListAvailable(PriorityPostParams priorityPostParams);

    Task<PriorityPostEntity> GetByPostId(int postId);

    Task<PriorityPostEntity> Create(PriorityPostEntity priorityPostEntity);
}