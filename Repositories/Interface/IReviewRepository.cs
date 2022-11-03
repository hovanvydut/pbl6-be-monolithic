using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Models.Entities;
using Monolithic.Models.ReqParams;

namespace Monolithic.Repositories.Interface;

public interface IReviewRepository
{
    Task<ReviewEntity> CreateReview(int userId, int postId, CreateReviewDTO dto);
    Task<PagedList<ReviewEntity>> GetAllReviewOfPost(int postId, ReviewParams reqParams);
}