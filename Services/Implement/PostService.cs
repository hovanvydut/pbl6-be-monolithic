using AutoMapper;
using Monolithic.Constants;
using Monolithic.Models.DTO;
using Monolithic.Models.Entities;
using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;

namespace Monolithic.Services.Implement;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepo;
    // private readonly IMediaRepository _mediaRepo;
    // private readonly PropertyRepository _propertyRepo;
    private readonly IMapper _mapper;

    public PostService(IPostRepository postRepo, IMapper mapper)
    {
        _postRepo = postRepo;
        _mapper = mapper;
    }

    // TODO: transaction here
    public async void CreatePost(CreatePostDTO createPostDTO)
    {

        // save post
        PostEntity postEntity = _mapper.Map<PostEntity>(createPostDTO);
        await _postRepo.CreatePost(postEntity);

        // save media
        List<MediaEntity> mediaEntityList = createPostDTO.Medias
                    .Select(m => _mapper.Map<MediaEntity>(m)).ToList();
        foreach (var media in mediaEntityList)
        {
            media.EntityType = (int) EntityType.POST;
        }
        
        // save properties
    }
}