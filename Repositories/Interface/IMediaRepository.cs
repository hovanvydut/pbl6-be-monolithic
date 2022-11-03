using Monolithic.Models.Entities;

namespace Monolithic.Repositories.Interface;

public interface IMediaRepository
{
    Task<MediaEntity> Create(MediaEntity mediaEntity);
    Task<List<MediaEntity>> Create(List<MediaEntity> mediaEntities);
    Task<List<MediaEntity>> GetAllMediaOfPost(int postId);
    Task<List<MediaEntity>> GetAllMediaOfReview(int reviewId);
    Task DeleteAllMediaOfPost(int postId);
}