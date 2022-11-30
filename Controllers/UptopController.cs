using static Monolithic.Constants.PermissionPolicy;
using Microsoft.AspNetCore.Authorization;
using Monolithic.Services.Interface;
using Monolithic.Models.ReqParams;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Constants;

namespace Monolithic.Controllers;

public class UptopController : BaseController
{
    private readonly IPriorityPostService _priorityPostService;

    public UptopController(IPriorityPostService priorityPostService)
    {
        _priorityPostService = priorityPostService;
    }

    [HttpPost]
    [Authorize(Roles = PostPermission.CreateUptop)]
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
    [Authorize(Roles = PostPermission.GetUptop)]
    public async Task<BaseResponse<PriorityPostDTO>> GetUpTopPost(int postId)
    {
        var priority = await _priorityPostService.GetByPostId(postId);
        return new BaseResponse<PriorityPostDTO>(priority);
    }

    [HttpGet("duplicate")]
    [Authorize(Roles = PostPermission.CheckDuplicateUptop)]
    public async Task<BaseResponse<List<PriorityPostDTO>>> GetDuplicateTimePriorityPost([FromQuery] PriorityPostParams priorityPostParams)
    {
        var duplicate = await _priorityPostService.GetPriorityDuplicateTime(priorityPostParams);
        return new BaseResponse<List<PriorityPostDTO>>(duplicate);
    }
}