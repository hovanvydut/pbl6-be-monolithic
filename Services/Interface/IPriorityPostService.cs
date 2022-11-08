using Monolithic.Models.DTO;
using Monolithic.Models.ReqParams;

namespace Monolithic.Services.Interface;

public interface IPriorityPostService
{
    Task<bool> CreatePriorityPost(int hostId, PriorityPostCreateDTO priorityCreateDTO);

    Task<PriorityPostDTO> GetByPostId(int postId);

    Task<List<PriorityPostDTO>> GetPriorityDuplicateTime(PriorityPostParams priorityPostParams);
}