using Monolithic.Models.Entities;

namespace Monolithic.Repositories.Interface;

public interface IUserProfileReposiory
{
    Task<UserProfileEntity> GetById(int id);

    Task<UserProfileEntity> GetByUserId(int userId);

    Task<UserProfileEntity> Create(UserProfileEntity userProfileEntity);

    Task<bool> IsInvalidNewProfile(UserProfileEntity userProfileEntity);

    Task<bool> Update(int userId, UserProfileEntity userProfileEntity);

    Task<bool> AddGold(int userId, long amount);

    Task<bool> UserMakePayment(int userId, double amount);
}