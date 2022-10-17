using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Monolithic.Common;
using Monolithic.Constants;
using Monolithic.Models.Context;
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
    private readonly DataContext _db;
    private readonly ICategoryRepository _categoryRepo;
    private readonly IAddressRepository _addressRepo;

    public PostService(IPostRepository postRepo, IMapper mapper,
                        IMediaRepository mediaRepo, IPropertyRepository propertyRepo,
                        DataContext db, ICategoryRepository categoryRepo,
                        IAddressRepository addressRepo)
    {
        _postRepo = postRepo;
        _mapper = mapper;
        _mediaRepo = mediaRepo;
        _propertyRepo = propertyRepo;
        _db = db;
        _categoryRepo = categoryRepo;
        _addressRepo = addressRepo;
    }

    public async Task<PostDTO> GetPostById(int id)
    {
        PostEntity postEntity = await _postRepo.GetPostById(id);
        if (postEntity == null) return null;

        PostDTO postDTO = _mapper.Map<PostDTO>(postEntity);
        List<MediaEntity> mediaEntityList = await _mediaRepo.GetAllMediaOfPost(postDTO.Id);
        postDTO.Medias = mediaEntityList.Select(m => _mapper.Map<MediaDTO>(m)).ToList();
        return postDTO;
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

    public async Task CreatePost(CreatePostDTO createPostDTO)
    {
        using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
        {
            try
            {
                // check if category exists
                CategoryEntity category = await _categoryRepo.GetHouseTypeById(createPostDTO.CategoryId);
                if (category == null)
                {
                    throw new BadHttpRequestException($"Category id = {createPostDTO.CategoryId} not found");
                }

                // check if address exists
                AddressWardEntity address = await _addressRepo.GetAddress(createPostDTO.AddressWardId);
                if (address == null)
                {
                    throw new BadHttpRequestException($"AddressWard id = {createPostDTO.AddressWardId} not found");
                }

                // save post
                PostEntity postEntity = _mapper.Map<PostEntity>(createPostDTO);
                PostEntity savedPostEntity = await _postRepo.CreatePost(postEntity);

                // save media
                List<MediaEntity> mediaEntityList = createPostDTO.Medias
                            .Select(m => _mapper.Map<MediaEntity>(m)).ToList();
                foreach (var media in mediaEntityList)
                {
                    media.EntityType = (int)EntityType.POST;
                    media.EntityId = savedPostEntity.Id;
                }
                await _mediaRepo.Create(mediaEntityList);

                // save properties
                List<PostPropertyEntity> postPropertyEntityList = new List<PostPropertyEntity>();
                foreach (var propertyId in createPostDTO.Properties)
                {
                    PropertyEntity propertyEntity = await _propertyRepo.GetPropertyById(propertyId);
                    if (propertyEntity == null)
                    {
                        throw new BadHttpRequestException($"Property id = {propertyId} not found");
                    }

                    PostPropertyEntity postPropertyEntity = new PostPropertyEntity
                    {
                        Post = savedPostEntity,
                        Property = propertyEntity
                    };
                    postPropertyEntityList.Add(postPropertyEntity);
                }
                await _propertyRepo.CreatePostProperty(postPropertyEntityList);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }
    }

    public async Task UpdatePost(int postId, UpdatePostDTO updatePostDTO)
    {
        using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
        {
            try
            {
                // check if category exists
                CategoryEntity category = await _categoryRepo.GetHouseTypeById(updatePostDTO.CategoryId);
                if (category == null)
                {
                    throw new BadHttpRequestException($"Category id = {updatePostDTO.CategoryId} not found");
                }

                // check if address exists
                AddressWardEntity address = await _addressRepo.GetAddress(updatePostDTO.AddressWardId);
                if (address == null)
                {
                    throw new BadHttpRequestException($"AddressWard id = {updatePostDTO.AddressWardId} not found");
                }

                // save post
                PostEntity updatedPostEntity = await _postRepo.UpdatePost(postId, updatePostDTO);

                // delete all old media
                await _mediaRepo.DeleteAllMediaOfPost(updatedPostEntity.Id);

                // save the new media
                List<MediaEntity> mediaEntityList = updatePostDTO.Medias
                    .Select(m => _mapper.Map<MediaEntity>(m)).ToList();
                foreach (var media in mediaEntityList)
                {
                    media.EntityType = (int)EntityType.POST;
                    media.EntityId = updatedPostEntity.Id;
                }
                await _mediaRepo.Create(mediaEntityList);

                // delete all old properties
                await _propertyRepo.DeleteAllPropertyOfPost(updatedPostEntity.Id);

                // save the new properties
                List<PostPropertyEntity> postPropertyEntityList = new List<PostPropertyEntity>();
                foreach (var propertyId in updatePostDTO.Properties)
                {
                    PropertyEntity propertyEntity = await _propertyRepo.GetPropertyById(propertyId);
                    if (propertyEntity == null)
                    {
                        throw new BadHttpRequestException($"Property id = {propertyId} not found");
                    }

                    PostPropertyEntity postPropertyEntity = new PostPropertyEntity
                    {
                        Post = updatedPostEntity,
                        Property = propertyEntity
                    };
                    postPropertyEntityList.Add(postPropertyEntity);
                }
                await _propertyRepo.CreatePostProperty(postPropertyEntityList);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }

    }

    public async Task DeletePost(int postId)
    {
        await _postRepo.DeletePost(postId);
    }

}