using Monolithic.Models.Entities;

namespace Monolithic.Repositories.Interface;

public interface IUserProfileReposiory
{
    Task<UserProfileEntity> GetByUserId(int userId);

    Task<UserProfileEntity> Create(UserProfileEntity userProfileEntity);

    Task<bool> IsInvalidNewProfile(UserProfileEntity userProfileEntity);
}