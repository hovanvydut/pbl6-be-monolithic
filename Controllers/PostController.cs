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

    [HttpPut]
    public async void Update()
    {

    }

    [HttpDelete]
    public async void Delete()
    {

    }
}