using Monolithic.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Monolithic.Models.Entities;
using Monolithic.Models.Context;

namespace Monolithic.Repositories.Implement;

public class UserProfileReposiory : IUserProfileReposiory
{
    private readonly DataContext _db;

    public UserProfileReposiory(DataContext db)
    {
        _db = db;
    }

    public async Task<UserProfileEntity> GetByUserId(int userId)
    {
        UserProfileEntity userProfile = await _db.UserProfiles
                                .Include(c => c.AddressWard)
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
}