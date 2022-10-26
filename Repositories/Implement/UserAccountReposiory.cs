using Monolithic.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Monolithic.Models.ReqParams;
using Monolithic.Models.Entities;
using Monolithic.Models.Context;
using Monolithic.Models.Common;
using Monolithic.Extensions;

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
                                .Include(c => c.UserProfile)
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

    public async Task<PagedList<UserAccountEntity>> GetAllUsers(UserParams userParams)
    {
        var users = _db.UserAccounts
                    .Include(u => u.UserProfile)
                    .ThenInclude(a => a.AddressWard.AddressDistrict.AddressProvince)
                    .OrderByDescending(r => r.CreatedAt)
                    .AsQueryable();
        if (!String.IsNullOrEmpty(userParams.SearchValue))
        {
            var searchValue = userParams.SearchValue.ToLower();
            users = users.Where(
                r => r.Email.Contains(searchValue) ||
                     r.UserProfile.DisplayName.Contains(searchValue) ||
                     r.UserProfile.PhoneNumber.Contains(searchValue) ||
                     r.UserProfile.IdentityNumber.Contains(searchValue) ||
                     r.UserProfile.Address.Contains(searchValue) ||
                     r.UserProfile.AddressWard.Name.Contains(searchValue) ||
                     r.UserProfile.AddressWard.AddressDistrict.Name.Contains(searchValue) ||
                     r.UserProfile.AddressWard.AddressDistrict.AddressProvince.Name.Contains(searchValue) 
            );
        }
        return await users.ToPagedList(userParams.PageNumber, userParams.PageSize);
    }
}