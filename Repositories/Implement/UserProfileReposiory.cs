using Monolithic.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Monolithic.Models.Entities;
using Monolithic.Models.Context;
using Monolithic.Models.Common;
using Monolithic.Constants;

namespace Monolithic.Repositories.Implement;

public class UserProfileReposiory : IUserProfileReposiory
{
    private readonly DataContext _db;

    public UserProfileReposiory(DataContext db)
    {
        _db = db;
    }

    public async Task<UserProfileEntity> GetById(int id)
    {
        UserProfileEntity userProfile = await _db.UserProfiles
                                .Include(c => c.UserAccount)
                                .FirstOrDefaultAsync(c => c.Id == id);
        if (userProfile == null) return null;
        _db.Entry(userProfile).State = EntityState.Detached;
        return userProfile;
    }

    public async Task<UserProfileEntity> GetByUserId(int userId)
    {
        UserProfileEntity userProfile = await _db.UserProfiles
                                .Include(c => c.UserAccount)
                                .FirstOrDefaultAsync(c => c.UserAccountId == userId);
        if (userProfile == null) return null;
        _db.Entry(userProfile).State = EntityState.Detached;
        return userProfile;
    }

    public async Task<UserProfileEntity> Create(UserProfileEntity userProfileEntity)
    {
        await _db.UserProfiles.AddAsync(userProfileEntity);
        await _db.SaveChangesAsync();
        return await GetByUserId(userProfileEntity.UserAccountId);
    }

    public async Task<bool> IsInvalidNewProfile(UserProfileEntity userProfileEntity)
    {
        return await _db.UserProfiles.AnyAsync(c =>
            c.PhoneNumber == userProfileEntity.PhoneNumber ||
            c.IdentityNumber == userProfileEntity.IdentityNumber
        );
    }

    public async Task<bool> Update(int userId, UserProfileEntity userProfileEntity)
    {
        UserProfileEntity userProfileDB = await GetByUserId(userId);
        if (userProfileDB == null) return false;

        if (userProfileDB.PhoneNumber != userProfileEntity.PhoneNumber
                        && await IsInvalidNewProfile(userProfileEntity))
            throw new BaseException(HttpCode.BAD_REQUEST, "Phone number is existed");

        userProfileDB.DisplayName = userProfileEntity.DisplayName;
        userProfileDB.Address = userProfileEntity.Address;
        userProfileDB.AddressWardId = userProfileEntity.AddressWardId;
        userProfileDB.PhoneNumber = userProfileEntity.PhoneNumber;
        userProfileDB.Avatar = userProfileEntity.Avatar;

        _db.UserProfiles.Update(userProfileDB);
        return await _db.SaveChangesAsync() >= 0;
    }

    public async Task<bool> AddGold(int userId, long amount)
    {
        UserProfileEntity userEntity = await this.GetById(userId);
        if (userEntity == null)
            throw new BaseException(HttpCode.BAD_REQUEST, "UserProfile id = " + userId + " doesn't found");
        // Credit from vnp response is multiplied 100
        userEntity.CurrentCredit += amount / 100;
        _db.UserProfiles.Update(userEntity);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> UserMakePayment(int userId, double amount)
    {
        UserProfileEntity userEntity = await this.GetById(userId);
        if (userEntity == null)
            throw new BaseException(HttpCode.BAD_REQUEST, "UserProfile id = " + userId + " doesn't found");
        if (userEntity.CurrentCredit < amount)
            throw new BaseException(HttpCode.BAD_REQUEST, "This user has not enough credit to pay");
        userEntity.CurrentCredit -= amount;
        _db.UserProfiles.Update(userEntity);
        return await _db.SaveChangesAsync() > 0;
    }
}