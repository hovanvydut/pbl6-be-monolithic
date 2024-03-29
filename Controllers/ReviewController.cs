using static Monolithic.Constants.PermissionPolicy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Constants;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Models.ReqParams;
using Monolithic.Services.Interface;

namespace Monolithic.Controllers;

public class ReviewController : BaseController
{

    private readonly IReviewService _reviewService;
    private readonly IAIService _aiService;

    public ReviewController(IReviewService reviewService, IAIService aiService)
    {
        _reviewService = reviewService;
        _aiService = aiService;
    }

    [HttpGet("post/{postId}")]
    public async Task<BaseResponse<PagedList<ReviewDTO>>> GetAllReviewOfPost(int postId, [FromQuery] ReviewParams reqParams)
    {
        PagedList<ReviewDTO> result = await _reviewService.GetAllReviewOfPost(postId, reqParams);
        return new BaseResponse<PagedList<ReviewDTO>>(result);
    }

    [HttpPost("post/{postId}")]
    [Authorize(Roles = ReviewPermission.Create)]
    public async Task<BaseResponse<bool>> CreateReview([FromBody] CreateReviewDTO dto, int postId)
    {
        ReqUser reqUser = HttpContext.Items["reqUser"] as ReqUser;
        await _reviewService.CreateReview(reqUser.Id, postId, dto);
        return new BaseResponse<bool>(true, HttpCode.OK);
    }

    [HttpGet("check-review/post/{postId}")]
    [Authorize(Roles = ReviewPermission.CheckCanReview)]
    public async Task<BaseResponse<bool>> CheckCanReview(int postId)
    {
        ReqUser reqUser = HttpContext.Items["reqUser"] as ReqUser;
        bool hasMet = await _reviewService.CheckCanReview(reqUser.Id, postId);
        return new BaseResponse<bool>(hasMet, HttpCode.OK);
    }

    [HttpPost("test-analyse-sentiment")]
    public async Task<BaseResponse<AnalyseReviewResDTO>> TestAnalyseReview([FromBody] string reviewContent)
    {
        AnalyseReviewResDTO dto = await _aiService.analyseReview(reviewContent);
        return new BaseResponse<AnalyseReviewResDTO>(dto);
    }
}