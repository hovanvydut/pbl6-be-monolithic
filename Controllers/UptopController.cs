using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Constants;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Models.ReqParams;
using Monolithic.Services.Interface;

namespace Monolithic.Controllers;

public class UptopController : BaseController
{
    private readonly IPriorityPostService _priorityPostService;

    public UptopController(IPriorityPostService priorityPostService)
    {
        _priorityPostService = priorityPostService;
    }

    [HttpPost]
    [Authorize]
    public async Task<BaseResponse<bool>> UpTopPost(PriorityPostCreateDTO priorityCreateDTO)
    {
        if (ModelState.IsValid)
        {
            var reqUser = HttpContext.Items["reqUser"] as ReqUser;
            var priorityPost = await _priorityPostService.CreatePriorityPost(reqUser.Id, priorityCreateDTO);
            if (priorityPost)
                return new BaseResponse<bool>(priorityPost, HttpCode.NO_CONTENT);
            else
                return new BaseResponse<bool>(priorityPost, HttpCode.BAD_REQUEST, "", false);
        }
        return new BaseResponse<bool>(false, HttpCode.BAD_REQUEST, "", false);
    }

    [HttpGet("{postId}")]
    [Authorize]
    public async Task<BaseResponse<PriorityPostDTO>> GetUpTopPost(int postId)
    {
        var priority = await _priorityPostService.GetByPostId(postId);
        return new BaseResponse<PriorityPostDTO>(priority);
    }

    [HttpGet("duplicate")]
    [Authorize]
    public async Task<BaseResponse<List<PriorityPostDTO>>> GetDuplicateTimePriorityPost([FromQuery] PriorityPostParams priorityPostParams)
    {
        var duplicate = await _priorityPostService.GetPriorityDuplicateTime(priorityPostParams);
        return new BaseResponse<List<PriorityPostDTO>>(duplicate);
    }

    // [HttpGet("/api/host/personal/uptop")]
    // [Authorize]
    // public async Task<BaseResponse<PriorityPostDTO>> GetListUptopPostPersonal([FromQuery] PostParams postParams)
    // {
    //     var priority = await _priorityPostService.GetByPostId(postId);
    //     return new BaseResponse<PriorityPostDTO>(priority);
    // }

    // [HttpGet("/api/host/{hostId}/uptop")]
    // [Authorize]
    // public async Task<BaseResponse<PriorityPostDTO>> GetListUptopPostByHostId(int hostId, [FromQuery] PostParams postParams)
    // {
    //     var priority = await _priorityPostService.GetByPostId(postId);
    //     return new BaseResponse<PriorityPostDTO>(priority);
    // }
}