using Monolithic.Models.DTO;

namespace Monolithic.Services.Interface;

public interface IAIService
{
    // Handle message
    Task<bool> handleReviewWorker(int reviewId);

    // Publish message
    Task<AnalyseReviewResDTO> analyseReview(String reviewStrv);
    Task<AnalyseReviewResDTO> analyseReview(int reviewId);
}