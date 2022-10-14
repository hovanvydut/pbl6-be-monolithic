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
        throw new NotImplementedException();   
    }

    [HttpPost]
    public async void Create([FromBody] CreatePostDTO reqData)
    {

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