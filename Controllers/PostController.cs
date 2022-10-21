using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
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

    [HttpGet]
    public async Task<BaseResponse<List<PostDTO>>> GetAll()
    {
        return new BaseResponse<List<PostDTO>>(await _postService.GetAllPost());
    }

    [HttpPost]
    public async Task<BaseResponse<string>> Create([FromBody] CreatePostDTO createPostDTO)
    {
        await _postService.CreatePost(createPostDTO);
        return new BaseResponse<string>("");
    }

    [HttpPut("{id}")]
    public async Task<BaseResponse<string>> Update(int id, [FromBody] UpdatePostDTO updatePostDTO)
    {
        await _postService.UpdatePost(id, updatePostDTO);
        return new BaseResponse<string>("");
    }

    [HttpDelete("{id}")]
    public async Task<BaseResponse<string>> Delete(int id)
    {
        await _postService.DeletePost(id);
        return new BaseResponse<string>("");
    }
}