using Monolithic.Models.Common;
using Monolithic.Models.Entities;
using Monolithic.Repositories.Interface;
using Monolithic.Models.Context;
using Microsoft.EntityFrameworkCore;
using Monolithic.Models.DTO;
using AutoMapper;
using Monolithic.Models.ReqParams;
using Monolithic.Extensions;

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

    public async Task<bool> DeletePost(int hostId, int postId)
    {
        PostEntity postEntity = await GetPostById(postId);
        if (postEntity.HostId != hostId) return false;
        if (postEntity != null)
        {
            postEntity.DeletedAt = new DateTime();
            return await _db.SaveChangesAsync() > 0;
        }
        return false;
    }

    public async Task<PostEntity> GetPostById(int id)
    {
        var postEntity = await _db.Posts.Where(p => p.DeletedAt == null && p.Id == id)
                            .Include(p => p.Category)
                            .Include(p => p.HostAccount.UserProfile)
                            .Include(p => p.AddressWard.AddressDistrict.AddressProvince)
                            .Include(p => p.PostProperties)
                            .ThenInclude(prop => prop.Property)
                            .FirstOrDefaultAsync();
        return postEntity;
    }

    public async Task<PostEntity> UpdatePost(int hostId, int postId, UpdatePostDTO updatePostDTO)
    {
        PostEntity existPostEntity = await GetPostById(postId);
        if (existPostEntity.HostId != hostId) return null;
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

    public async Task<PagedList<PostEntity>> GetWithParamsInSearchAndFilter(PostSearchFilterParams postParams, IEnumerable<int> priorityIds)
    {
        var posts = _db.Posts.Include(p => p.Category)
                            .Include(p => p.AddressWard.AddressDistrict.AddressProvince)
                            .Include(p => p.PostProperties)
                            .ThenInclude(prop => prop.Property)
                            .OrderByDescending(c => c.CreatedAt)
                            .Where(p => p.DeletedAt == null)
                            .Where(p => !priorityIds.Contains(p.Id));

        if (postParams.AddressWardId > 0)
        {
            posts = posts.Where(p => p.AddressWardId == postParams.AddressWardId);
        }

        if (postParams.CategoryId > 0)
        {
            posts = posts.Where(p => p.CategoryId == postParams.CategoryId);
        }

        if (!String.IsNullOrEmpty(postParams.SearchValue))
        {
            var searchValue = postParams.SearchValue.ToLower();
            posts = posts.Where(
                p => p.Title.ToLower().Contains(searchValue) ||
                     p.Description.ToLower().Contains(searchValue) ||
                     p.Address.ToLower().Contains(searchValue) ||
                     p.AddressWard.Name.ToLower().Contains(searchValue) ||
                     p.AddressWard.AddressDistrict.Name.ToLower().Contains(searchValue) ||
                     p.AddressWard.AddressDistrict.AddressProvince.Name.ToLower().Contains(searchValue) ||
                     p.Category.Name.ToLower().Contains(searchValue)
            );
        }

        if (postParams.MaxPrice > 0 && postParams.MaxPrice > postParams.MinPrice)
        {
            posts = posts.Where(
                p => p.Price >= postParams.MinPrice &&
                     p.Price <= postParams.MaxPrice
            );
        }

        if (postParams.MaxArea > 0 && postParams.MaxArea > postParams.MinPrice)
        {
            posts = posts.Where(
                p => p.Area >= postParams.MinArea &&
                     p.Area <= postParams.MaxArea
            );
        }

        if (!String.IsNullOrEmpty(postParams.Properties))
        {
            var properties = postParams.Properties.Split(",").Select(c => Convert.ToInt32(c));
            foreach (var prop in properties)
            {
                posts = posts.Where(
                    p => p.PostProperties.Select(c => c.PropertyId).Contains(prop)
                );
            }
        }
        return await posts.ToPagedList(postParams.PageNumber, postParams.PageSize);
    }

    public async Task<PagedList<PostEntity>> GetWithParamsInTableAndList(int hostId, PostTableListParams postParams, IEnumerable<int> priorityIds)
    {
        var posts = _db.Posts.Include(p => p.Category)
                            .Include(p => p.AddressWard.AddressDistrict.AddressProvince)
                            .Include(p => p.PostProperties)
                            .ThenInclude(prop => prop.Property)
                            .OrderByDescending(c => c.CreatedAt)
                            .AsQueryable();
        if (postParams.Deleted)
        {
            posts = posts.Where(p => p.DeletedAt != null);
        }
        else
        {
            posts = posts.Where(p => p.DeletedAt == null);
        }

        if (hostId > 0)
        {
            posts = posts.Where(p => p.HostId == hostId);
        }

        if (priorityIds.Count() > 0)
        {
            posts = posts.Where(p => priorityIds.Contains(p.Id));
        }

        if (!String.IsNullOrEmpty(postParams.SearchValue))
        {
            var searchValue = postParams.SearchValue.ToLower();
            posts = posts.Where(
                p => p.Title.ToLower().Contains(searchValue) ||
                     p.Description.ToLower().Contains(searchValue) ||
                     p.Address.ToLower().Contains(searchValue) ||
                     p.AddressWard.Name.ToLower().Contains(searchValue) ||
                     p.AddressWard.AddressDistrict.Name.ToLower().Contains(searchValue) ||
                     p.AddressWard.AddressDistrict.AddressProvince.Name.ToLower().Contains(searchValue) ||
                     p.Category.Name.ToLower().Contains(searchValue)
            );
        }
        return await posts.ToPagedList(postParams.PageNumber, postParams.PageSize);
    }

    public async Task<List<PostEntity>> GetRelatedPost(RelatedPostParams relatedPostParams)
    {
        var posts = await _db.Posts.Include(p => p.Category)
                            .Include(p => p.AddressWard.AddressDistrict.AddressProvince)
                            .Include(p => p.PostProperties)
                            .ThenInclude(prop => prop.Property)
                            .Where(p => p.DeletedAt == null)
                            .Where(p => p.Id != relatedPostParams.CurrentPostId)
                            .Where(p => p.AddressWardId == relatedPostParams.AddressWardId).ToListAsync();
        // Use guid sort for random
        return posts.OrderBy(_ => Guid.NewGuid()).Take(relatedPostParams.Quantity).ToList();
    }
}
