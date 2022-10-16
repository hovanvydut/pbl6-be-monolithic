using Monolithic.Models.DTO;

namespace Monolithic.Services.Interface;

public interface IPostService
{
    Task<PostDTO> GetPostById(int id);
    Task<List<PostDTO>> GetAllPost();
    Task CreatePost(CreatePostDTO createPostDTO);
}