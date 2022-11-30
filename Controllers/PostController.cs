using static Monolithic.Constants.PermissionPolicy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Constants;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Models.ReqParams;
using Monolithic.Services.Interface;

namespace Monolithic.Controllers;

public class PostController : BaseController
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet("{id}")]
    public async Task<BaseResponse<PostDTO>> GetPostById(int id)
    {
        var reqUser = HttpContext.Items["reqUser"] as ReqUser;
        var userId = reqUser != null ? reqUser.Id : 0;
        PostDTO post = await _postService.GetPostById(id, userId);
        return new BaseResponse<PostDTO>(post);
    }

    [HttpGet]
    public async Task<BaseResponse<PagedList<PostDTO>>> GetWithParams([FromQuery] PostSearchFilterParams postParams)
    {
        var reqUser = HttpContext.Items["reqUser"] as ReqUser;
        PagedList<PostDTO> posts = null;
        if (reqUser == null)
        {
            posts = await _postService.GetWithParamsInSearchAndFilter(0, postParams);
        }
        else
        {
            posts = await _postService.GetWithParamsInSearchAndFilter(reqUser.Id, postParams);
        }
        return new BaseResponse<PagedList<PostDTO>>(posts);
    }

    [HttpGet("related")]
    public async Task<BaseResponse<List<PostDTO>>> GetRelatedPosts([FromQuery] RelatedPostParams relatedPostParams)
    {
        var posts = await _postService.GetRelatedPost(relatedPostParams);
        return new BaseResponse<List<PostDTO>>(posts);
    }

    [HttpGet("/api/host/personal/post")]
    [Authorize(Roles = PostPermission.ViewAllPersonal)]
    public async Task<BaseResponse<PagedList<PostDTO>>> GetWithParamsPersonal([FromQuery] PostTableListParams postParams)
    {
        var reqUser = HttpContext.Items["reqUser"] as ReqUser;
        var posts = await _postService.GetWithParamsInTableAndList(reqUser.Id, 0, postParams);
        return new BaseResponse<PagedList<PostDTO>>(posts);
    }

    [HttpGet("/api/host/{hostId}/post")]
    public async Task<BaseResponse<PagedList<PostDTO>>> GetWithParamsByHostId(int hostId, [FromQuery] PostTableListParams postParams)
    {
        if (hostId <= 0)
        {
            return new BaseResponse<PagedList<PostDTO>>(null, HttpCode.BAD_REQUEST, "hostId is invalid", false);
        }
        var reqUser = HttpContext.Items["reqUser"] as ReqUser;
        PagedList<PostDTO> posts = null;
        if (reqUser == null)
        {
            posts = await _postService.GetWithParamsInTableAndList(hostId, 0, postParams);
        }
        else
        {
            posts = await _postService.GetWithParamsInTableAndList(hostId, reqUser.Id, postParams);
        }
        return new BaseResponse<PagedList<PostDTO>>(posts);
    }

    [HttpPost]
    [Authorize(Roles = PostPermission.Create)]
    public async Task<BaseResponse<string>> Create([FromBody] CreatePostDTO createPostDTO)
    {
        if (ModelState.IsValid)
        {
            var reqUser = HttpContext.Items["reqUser"] as ReqUser;
            await _postService.CreatePost(reqUser.Id, createPostDTO);
            return new BaseResponse<string>("");
        }
        return new BaseResponse<string>("", HttpCode.BAD_REQUEST, "Model state is not valid", false);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = PostPermission.Update)]
    public async Task<BaseResponse<string>> Update(int id, [FromBody] UpdatePostDTO updatePostDTO)
    {
        if (ModelState.IsValid)
        {
            var reqUser = HttpContext.Items["reqUser"] as ReqUser;
            await _postService.UpdatePost(reqUser.Id, id, updatePostDTO);
            return new BaseResponse<string>("");
        }
        return new BaseResponse<string>("", HttpCode.BAD_REQUEST, "Model state is not valid", false);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = PostPermission.Delete)]
    public async Task<BaseResponse<string>> Delete(int id)
    {
        var reqUser = HttpContext.Items["reqUser"] as ReqUser;
        await _postService.DeletePost(reqUser.Id, id);
        return new BaseResponse<string>("");
    }
}