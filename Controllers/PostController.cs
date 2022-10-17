using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult<PostDTO>> GetPostById(int id)
    {
        return await _postService.GetPostById(id);
    }

    [HttpGet]
    public async Task<ActionResult<List<PostDTO>>> GetAll()
    {
        return await _postService.GetAllPost();
    }

    [HttpPost]
    public async Task Create([FromBody] CreatePostDTO createPostDTO)
    {
        await _postService.CreatePost(createPostDTO);
    }

    [HttpPut("{id}")]
    public async Task Update(int id, [FromBody] UpdatePostDTO updatePostDTO)
    {
        await _postService.UpdatePost(id, updatePostDTO);
    }

    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
        await _postService.DeletePost(id);
    }
}