using Monolithic.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Monolithic.Models.ReqParams;
using Monolithic.Models.Entities;
using Monolithic.Models.Context;
using Monolithic.Models.Common;
using Monolithic.Extensions;

namespace Monolithic.Repositories.Implement;

public class RoleRepository : IRoleRepository
{
    private readonly DataContext _db;

    public RoleRepository(DataContext db)
    {
        _db = db;
    }

    public async Task<bool> CreateRole(RoleEntity createRole)
    {
        await _db.Roles.AddAsync(createRole);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateRole(int roleId, RoleEntity updateRole)
    {
        RoleEntity roleDB = await GetRoleById(roleId);
        if (roleDB == null) return false;
        updateRole.Id = roleId;
        updateRole.CreatedAt = roleDB.CreatedAt;
        _db.Roles.Update(updateRole);
        return await _db.SaveChangesAsync() >= 0;
    }

    public async Task<PagedList<RoleEntity>> GetWithParams(RoleParams roleParams)
    {
        var roles = _db.Roles.OrderBy(r => r.CreatedAt).AsQueryable();
        if (!String.IsNullOrEmpty(roleParams.SearchValue))
        {
            var searchValue = roleParams.SearchValue.ToLower();
            roles = roles.Where(
                r => r.Name.Contains(searchValue) ||
                     r.Description.Contains(searchValue)
            );
        }
        return await roles.ToPagedList(roleParams.PageNumber, roleParams.PageSize);
    }

    public async Task<List<RoleEntity>> GetAllRoles()
    {
        return await _db.Roles.ToListAsync();
    }

    public async Task<RoleEntity> GetRoleById(int roleId)
    {
        RoleEntity role = await _db.Roles
                                .Include(c => c.Permissions)
                                .FirstOrDefaultAsync(c => c.Id == roleId);
        if (role == null) return null;
        _db.Entry(role).State = EntityState.Detached;
        return role;
    }

    public async Task<List<PermissionEntity>> GetPermissionByRoleId(int roleId)
    {
        return await _db.Permissions.Where(c => c.RoleId == roleId).ToListAsync();
    }

    public async Task<bool> AddPermissionForRole(PermissionEntity createPermission)
    {
        await _db.Permissions.AddAsync(createPermission);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemovePermissionForRole(int permissionId, int roleId)
    {
        PermissionEntity permissionDB = await _db.Permissions
                                            .FirstOrDefaultAsync(c => c.Id == permissionId &&
                                                                      c.RoleId == roleId);
        if (permissionDB == null) return false;
        _db.Entry(permissionDB).State = EntityState.Detached;
        _db.Permissions.Remove(permissionDB);
        return await _db.SaveChangesAsync() > 0;
    }
}