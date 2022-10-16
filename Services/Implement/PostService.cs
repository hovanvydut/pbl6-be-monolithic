using AutoMapper;
using Monolithic.Common;
using Monolithic.Constants;
using Monolithic.Models.DTO;
using Monolithic.Models.Entities;
using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;

namespace Monolithic.Services.Implement;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepo;
    private readonly IMediaRepository _mediaRepo;
    private readonly IPropertyRepository _propertyRepo;
    private readonly IMapper _mapper;

    public PostService(IPostRepository postRepo, IMapper mapper,
                        IMediaRepository mediaRepo, IPropertyRepository propertyRepo)
    {
        _postRepo = postRepo;
        _mapper = mapper;
        _mediaRepo = mediaRepo;
        _propertyRepo = propertyRepo;
    }

    public async Task<PostDTO> GetPostById(int id)
    {
        PostEntity postEntity = await _postRepo.GetPostById(id);

        if (postEntity == null) return null;
        return _mapper.Map<PostDTO>(postEntity);
    }

    public async Task<List<PostDTO>> GetAllPost()
    {
        List<PostEntity> postEntityList = await _postRepo.GetAllPost();
        List<PostDTO> postDTOList = postEntityList.Select(p => _mapper.Map<PostDTO>(p)).ToList();

        // attach media on PostDTO
        foreach (var postDTO in postDTOList)
        {
            List<MediaEntity> mediaEntityList = await _mediaRepo.GetAllMediaOfPost(postDTO.Id);
            postDTO.Medias = mediaEntityList.Select(m => _mapper.Map<MediaDTO>(m)).ToList();
        }
        return postDTOList;
    }

    // TODO: transaction here
    public async Task CreatePost(CreatePostDTO createPostDTO)
    {

        // save post
        PostEntity postEntity = _mapper.Map<PostEntity>(createPostDTO);
        PostEntity savedPostEntity = await _postRepo.CreatePost(postEntity);

        // save media
        List<MediaEntity> mediaEntityList = createPostDTO.Medias
                    .Select(m => _mapper.Map<MediaEntity>(m)).ToList();
        foreach (var media in mediaEntityList)
        {
            media.EntityType = (int) EntityType.POST;
            media.EntityId = savedPostEntity.Id;
        }
        await _mediaRepo.Create(mediaEntityList);
        
        // save properties
        List<PostPropertyEntity> postPropertyEntityList = new List<PostPropertyEntity>();
        foreach (var propertyId in createPostDTO.Properties)
        {
            PropertyEntity propertyEntity = await _propertyRepo.GetPropertyById(propertyId);
            PostPropertyEntity postPropertyEntity = new PostPropertyEntity
            {
                Post = savedPostEntity,
                Property = propertyEntity
            };
            postPropertyEntityList.Add(postPropertyEntity);
        }
        await _propertyRepo.CreatePostProperty(postPropertyEntityList);
    }
}