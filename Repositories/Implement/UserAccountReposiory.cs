using Monolithic.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Monolithic.Models.Entities;
using Monolithic.Models.Context;

namespace Monolithic.Repositories.Implement;

public class UserAccountReposiory : IUserAccountReposiory
{
    private readonly DataContext _db;

    public UserAccountReposiory(DataContext db)
    {
        _db = db;
    }

    public async Task<UserAccountEntity> GetByEmail(string email)
    {
        UserAccountEntity userAccount = await _db.UserAccounts
                                .Include(c => c.Role)
                                .FirstOrDefaultAsync(c => c.Email == email);
        if (userAccount == null) return null;
        _db.Entry(userAccount).State = EntityState.Detached;
        return userAccount;
    }

    public async Task<UserAccountEntity> GetById(int id)
    {
        UserAccountEntity userAccount = await _db.UserAccounts
                                .Include(c => c.Role)
                                .FirstOrDefaultAsync(c => c.Id == id);
        if (userAccount == null) return null;
        _db.Entry(userAccount).State = EntityState.Detached;
        return userAccount;
    }

    public async Task<UserAccountEntity> Create(UserAccountEntity userAccountEntity)
    {
        await _db.UserAccounts.AddAsync(userAccountEntity);
        await _db.SaveChangesAsync();
        return await GetById(userAccountEntity.Id);
    }

    public async Task<bool> Update(int id, UserAccountEntity userAccountEntity)
    {
        UserAccountEntity userAccountDB = await GetById(id);
        if (userAccountDB == null) return false;
        userAccountEntity.Id = id;
        _db.UserAccounts.Update(userAccountEntity);
        return await _db.SaveChangesAsync() >= 0;
    }
}