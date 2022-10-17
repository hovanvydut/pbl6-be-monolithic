using Monolithic.Models.Common;
using Monolithic.Models.Entities;
using Monolithic.Repositories.Interface;
using Monolithic.Models.Context;
using Microsoft.EntityFrameworkCore;
using Monolithic.Models.DTO;
using AutoMapper;

namespace Monolithic.Repositories.Implement;

public class PostRepository : IPostRepository
{
    private readonly DataContext _db;
    private readonly IMapper _mapper;

    public PostRepository(DataContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<PostEntity> CreatePost(PostEntity postEntity)
    {
        await _db.Posts.AddAsync(postEntity);
        await _db.SaveChangesAsync();
        return await GetPostById(postEntity.Id);
    }

    public async Task DeletePost(int postId)
    {
        PostEntity postEntity = await GetPostById(postId);
        if (postEntity != null)
        {
            postEntity.DeletedAt = new DateTime();
            await _db.SaveChangesAsync();
        }
    }

    public async Task<List<PostEntity>> GetAllPost()
    {
        return await _db.Posts.Where(p => p.DeletedAt == null)
                            .Include(p => p.Category)
                            .Include(p => p.AddressWard.AddressDistrict.AddressProvince)
                            .Include(p => p.PostProperties)
                            .ThenInclude(prop => prop.Property)
                            .ToListAsync();
    }

    public async Task<PostEntity> GetPostById(int id)
    {
        var postEntity = await _db.Posts.Where(p => p.DeletedAt == null && p.Id == id)
                            .Include(p => p.Category)
                            .Include(p => p.AddressWard.AddressDistrict.AddressProvince)
                            .Include(p => p.PostProperties)
                            .ThenInclude(prop => prop.Property)
                            .FirstOrDefaultAsync();
        return postEntity;
    }

    public async Task<PostEntity> UpdatePost(int postId, UpdatePostDTO updatePostDTO)
    {
        PostEntity existPostEntity = await GetPostById(postId);
        if (existPostEntity != null)
        {
            existPostEntity.Title = updatePostDTO.Title;
            existPostEntity.Description = updatePostDTO.Description;
            existPostEntity.Area = updatePostDTO.Area;
            existPostEntity.AddressWardId = updatePostDTO.AddressWardId;
            existPostEntity.Price = updatePostDTO.Price;
            existPostEntity.PrePaidPrice = updatePostDTO.PrePaidPrice;
            existPostEntity.CategoryId = updatePostDTO.CategoryId;
            existPostEntity.LimitTenant = updatePostDTO.LimitTenant;
            await _db.SaveChangesAsync();
        }
        return existPostEntity;
    }
}
