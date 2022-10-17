using Monolithic.Models.Entities;

namespace Monolithic.Repositories.Interface;

public interface IMediaRepository
{
    Task<MediaEntity> Create(MediaEntity mediaEntity);
    Task<List<MediaEntity>> Create(List<MediaEntity> mediaEntities);
    Task<List<MediaEntity>> GetAllMediaOfPost(int postId);
    Task DeleteAllMediaOfPost(int postId);
}