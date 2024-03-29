using Monolithic.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Monolithic.Models.ReqParams;
using Monolithic.Models.Entities;
using Monolithic.Models.Context;
using Monolithic.Models.Common;
using Monolithic.Extensions;

namespace Monolithic.Repositories.Implement;

public class PriorityPostRepository : IPriorityPostRepository
{
    private readonly DataContext _db;
    public PriorityPostRepository(DataContext db)
    {
        _db = db;
    }
    public async Task<List<PriorityPostEntity>> GetListDuplicateTime(PriorityPostParams priorityPostParams)
    {
        var listPriorities = _db.PriorityPosts.Include(c => c.Post).AsQueryable();
        if (priorityPostParams.AddressWardId > 0)
            listPriorities = listPriorities.Where(c => c.Post.AddressWardId == priorityPostParams.AddressWardId);

        if (priorityPostParams.StartTime != null && priorityPostParams.EndTime != null)
            listPriorities = listPriorities.Where(c =>
                            (c.StartTime <= priorityPostParams.StartTime && priorityPostParams.StartTime <= c.EndTime) ||
                            (c.StartTime <= priorityPostParams.EndTime && priorityPostParams.EndTime <= c.EndTime));
        return await listPriorities.ToListAsync();
    }

    public async Task<PagedList<PriorityPostEntity>> GetListAvailable(PriorityPostParams priorityPostParams)
    {
        var now = DateTime.UtcNow;
        var listPriorities = _db.PriorityPosts.Include(c => c.Post)
                                .Include(p => p.Post.Category)
                                .Include(p => p.Post.AddressWard.AddressDistrict.AddressProvince)
                                .Include(p => p.Post.PostProperties)
                                .ThenInclude(prop => prop.Property)
                                .OrderBy(c => c.CreatedAt)
                                .Where(c => now <= c.EndTime);
        if (priorityPostParams.AddressWardId > 0)
            listPriorities = listPriorities.Where(c => c.Post.AddressWardId == priorityPostParams.AddressWardId);
        return await listPriorities.ToPagedList(priorityPostParams.PageNumber, priorityPostParams.PageSize);
    }

    public async Task<PriorityPostEntity> GetByPostId(int postId)
    {
        var now = DateTime.UtcNow;
        PriorityPostEntity priorityPost = await _db.PriorityPosts
                                .Include(c => c.Post)
                                .FirstOrDefaultAsync(c => c.PostId == postId && now <= c.EndTime);
        if (priorityPost == null) return null;
        _db.Entry(priorityPost).State = EntityState.Detached;
        return priorityPost;
    }

    public async Task<PriorityPostEntity> Create(PriorityPostEntity priorityPostEntity)
    {
        await _db.PriorityPosts.AddAsync(priorityPostEntity);
        await _db.SaveChangesAsync();
        return await GetByPostId(priorityPostEntity.PostId);
    }

    public async Task<List<int>> GetAllPostIdAvailable()
    {
        var now = DateTime.UtcNow;
        return await _db.PriorityPosts
                        .Where(c => now <= c.EndTime)
                        .Select(c => c.PostId).ToListAsync();
    }
}