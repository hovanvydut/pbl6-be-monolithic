using AutoMapper;
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
    private readonly IMapper _mapper;

    public PostController(IPostService postService, IMapper mapper)
    {
        this._postService = postService;
        this._mapper = mapper;
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
        var posts = await _postService.GetPostWithParams(postParams);
        return new BaseResponse<PagedList<PostDTO>>(posts);
    }

    [HttpPost]
    public async Task<BaseResponse<string>> Create([FromBody] CreatePostDTO createPostDTO)
    {
        if (ModelState.IsValid)
        {
            await _postService.CreatePost(createPostDTO);
            return new BaseResponse<string>("");
        }
        return new BaseResponse<string>("", HttpCode.BAD_REQUEST, "Model state is not valid", false);
    }

    [HttpPut("{id}")]
    public async Task<BaseResponse<string>> Update(int id, [FromBody] UpdatePostDTO updatePostDTO)
    {
        if (ModelState.IsValid)
        {
            await _postService.UpdatePost(id, updatePostDTO);
            return new BaseResponse<string>("");
        }
        return new BaseResponse<string>("", HttpCode.BAD_REQUEST, "Model state is not valid", false);
    }

    [HttpDelete("{id}")]
    public async Task<BaseResponse<string>> Delete(int id)
    {
        await _postService.DeletePost(id);
        return new BaseResponse<string>("");
    }
}