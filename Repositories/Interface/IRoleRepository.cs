using Monolithic.Models.ReqParams;
using Monolithic.Models.Entities;
using Monolithic.Models.Common;

namespace Monolithic.Repositories.Interface;

public interface IRoleRepository
{
    Task<bool> CreateRole(RoleEntity createRole);

    Task<bool> UpdateRole(int roleId, RoleEntity updateRole);

    Task<PagedList<RoleEntity>> GetAllRoles(RoleParams roleParams);

    Task<RoleEntity> GetRoleById(int roleId);

    Task<List<PermissionEntity>> GetPermissionByRoleId(int roleId);

    Task<bool> AddPermissionForRole(PermissionEntity createPermission);

    Task<bool> RemovePermissionForRole(int permissionId, int roleId);
}