using Monolithic.Models.DTO;

namespace Monolithic.Services.Interface;

public interface IAIService
{
    Task<AnalyseReviewResDTO> analyseReview(String reviewStrv);
    Task<AnalyseReviewResDTO> analyseReview(int reviewId);
}