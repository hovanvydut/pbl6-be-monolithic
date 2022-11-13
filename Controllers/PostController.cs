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
    private readonly IPriorityPostService _priorityPostService;

    public PostController(IPostService postService,
                          IPriorityPostService priorityPostService)
    {
        _postService = postService;
        _priorityPostService = priorityPostService;
    }

    [HttpGet("{id}")]
    public async Task<BaseResponse<PostDTO>> GetPostById(int id)
    {
        return new BaseResponse<PostDTO>(await _postService.GetPostById(id));
    }

    [HttpGet("all")]
    public async Task<BaseResponse<List<PostDTO>>> GetAll()
    {
        var post = await _postService.GetAllPost();
        return new BaseResponse<List<PostDTO>>(post);
    }

    [HttpGet]
    public async Task<BaseResponse<PagedList<PostDTO>>> GetWithParams([FromQuery] PostParams postParams)
    {
        var reqUser = HttpContext.Items["reqUser"] as ReqUser;
        var posts = new PagedList<PostDTO>();
        if (reqUser == null)
        {
            posts = await _postService.GetPostWithParams(0, 0, postParams);
        }
        else
        {
            posts = await _postService.GetPostWithParams(0, reqUser.Id, postParams);
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
    [Authorize]
    public async Task<BaseResponse<PagedList<PostDTO>>> GetWithParamsPersonal([FromQuery] PostParams postParams)
    {
        var reqUser = HttpContext.Items["reqUser"] as ReqUser;
        var posts = await _postService.GetPostWithParams(reqUser.Id, 0, postParams);
        return new BaseResponse<PagedList<PostDTO>>(posts);
    }

    [HttpGet("/api/host/{hostId}/post")]
    public async Task<BaseResponse<PagedList<PostDTO>>> GetWithParamsByHostId(int hostId, [FromQuery] PostParams postParams)
    {
        if (hostId <= 0)
        {
            return new BaseResponse<PagedList<PostDTO>>(null, HttpCode.BAD_REQUEST, "hostId is invalid", false);
        }
        var reqUser = HttpContext.Items["reqUser"] as ReqUser;
        var posts = new PagedList<PostDTO>();
        if (reqUser == null)
        {
            posts = await _postService.GetPostWithParams(hostId, 0, postParams);
        }
        else
        {
            posts = await _postService.GetPostWithParams(hostId, reqUser.Id, postParams);
        }
        return new BaseResponse<PagedList<PostDTO>>(posts);
    }

    [HttpPost]
    [Authorize]
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
    [Authorize]
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
    [Authorize]
    public async Task<BaseResponse<string>> Delete(int id)
    {
        var reqUser = HttpContext.Items["reqUser"] as ReqUser;
        await _postService.DeletePost(reqUser.Id, id);
        return new BaseResponse<string>("");
    }

    [HttpPost("uptop")]
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

    [HttpGet("uptop/{postId}")]
    [Authorize]
    public async Task<BaseResponse<PriorityPostDTO>> GetUpTopPost(int postId)
    {
        var priority = await _priorityPostService.GetByPostId(postId);
        return new BaseResponse<PriorityPostDTO>(priority);
    }

    [HttpGet("uptop/duplicate")]
    [Authorize]
    public async Task<BaseResponse<List<PriorityPostDTO>>> GetDuplicateTimePriorityPost([FromQuery] PriorityPostParams priorityPostParams)
    {
        var duplicate = await _priorityPostService.GetPriorityDuplicateTime(priorityPostParams);
        return new BaseResponse<List<PriorityPostDTO>>(duplicate);
    }
}