using Monolithic.Models.Common;
using Monolithic.Models.Entities;
using Monolithic.Repositories.Interface;
using Monolithic.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace Monolithic.Repositories.Implement;

public class PostRepository : IPostRepository
{
    private readonly DataContext _db;

    public PostRepository(DataContext db)
    {
        _db = db;
    }

    public async Task<PostEntity> CreatePost(PostEntity postEntity)
    {
        await _db.Posts.AddAsync(postEntity);
        await _db.SaveChangesAsync();
        return await GetPostById(postEntity.Id);
    }

    public async Task<List<PostEntity>> GetAllPost()
    {
        return await _db.Posts.Where(p => p.DeletedAt == null)
                            .Include(p => p.Category)
                            .Include(p => p.AddressWard)
                            .Include(p => p.PostProperties)
                            .ThenInclude(prop => prop.Property)
                            .ToListAsync();
    }

    public async Task<PostEntity> GetPostById(int id)
    {
        var postEntity = await _db.Posts.Where(p => p.DeletedAt == null && p.Id == id)
                            .Include(p => p.Category)
                            .Include(p => p.AddressWard)
                            .Include(p => p.PostProperties)
                            .ThenInclude(prop => prop.Property)
                            .FirstOrDefaultAsync();
        return postEntity;
    }
}
