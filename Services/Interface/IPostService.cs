using Monolithic.Models.DTO;

namespace Monolithic.Services.Interface;

public interface IPostService
{
    void CreatePost(CreatePostDTO createPostDTO);
}