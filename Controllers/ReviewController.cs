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

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet("post/{postId}")]   
    public async Task<BaseResponse<PagedList<ReviewDTO>>> GetAllReviewOfPost(int postId, [FromQuery] ReviewParams reqParams)
    {
        PagedList<ReviewDTO> result = await _reviewService.GetAllReviewOfPost(postId, reqParams);
        return new BaseResponse<PagedList<ReviewDTO>>(result);   
    }

    [HttpPost("post/{postId}")]
    [Authorize]
    public async Task<BaseResponse<bool>> CreateReview([FromBody] CreateReviewDTO dto, int postId)
    {
        ReqUser reqUser = HttpContext.Items["reqUser"] as ReqUser;
        await _reviewService.CreateReview(reqUser.Id, postId, dto);
        return new BaseResponse<bool>(true, HttpCode.OK);
    }

    [HttpGet("check-review/post/{postId}")]
    [Authorize]
    public async Task<BaseResponse<bool>> CheckCanReview(int postId)
    {
        ReqUser reqUser = HttpContext.Items["reqUser"] as ReqUser;
        bool hasMet = await _reviewService.CheckCanReview(reqUser.Id, postId);
        return new BaseResponse<bool>(hasMet, HttpCode.OK);
    }
}