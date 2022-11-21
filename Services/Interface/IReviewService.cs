using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Models.ReqParams;

namespace Monolithic.Services.Interface;

public interface IReviewService
{
    Task<PagedList<ReviewDTO>> GetAllReviewOfPost(int postId, ReviewParams reqParams);
    Task CreateReview(int userId, int postId, CreateReviewDTO dto);
    Task<double> GetAverageRatingOfPost(int postId);
    Task<bool> CheckCanReview(int userId, int postId);
}