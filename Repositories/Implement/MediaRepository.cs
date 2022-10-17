using Microsoft.EntityFrameworkCore;
using Monolithic.Constants;
using Monolithic.Models.Context;
using Monolithic.Models.Entities;
using Monolithic.Repositories.Interface;

namespace Monolithic.Repositories.Implement;

public class MediaRepository : IMediaRepository
{
    private readonly DataContext _db;

    public MediaRepository(DataContext db)
    {
        _db = db;
    }

    public async Task<MediaEntity> Create(MediaEntity mediaEntity)
    {
        await _db.Medias.AddAsync(mediaEntity);
        await _db.SaveChangesAsync();
        return mediaEntity;
    }

    public async Task<List<MediaEntity>> Create(List<MediaEntity> mediaEntities)
    {
        List<MediaEntity> result = new List<MediaEntity>();
        foreach (var mediaEntity in mediaEntities)
        {
            result.Add(await Create(mediaEntity));
        }
        return result;
    }

    public async Task DeleteAllMediaOfPost(int postId)
    {
        List<MediaEntity> mediaList = await _db.Medias.Where(m => m.EntityType == EntityType.POST && m.EntityId ==  postId).ToListAsync();
        foreach (var media in mediaList)
        {
            _db.Medias.Remove(media);
        }
       await _db.SaveChangesAsync();
    }

    public async Task<List<MediaEntity>> GetAllMediaOfPost(int postId)
    {
        return await _db.Medias.Where(m => m.EntityId == postId
                                        && m.EntityType == EntityType.POST)
                                        .ToListAsync();
    }
}