using Monolithic.Models.Entities;

namespace Monolithic.Repositories.Interface;

public interface IRoleRepository
{
    Task<bool> CreateRole(RoleEntity createRole);

    Task<bool> UpdateRole(int roleId, RoleEntity updateRole);

    Task<List<RoleEntity>> GetAllRoles();

    Task<RoleEntity> GetRoleById(int roleId);

    Task<List<PermissionEntity>> GetPermissionByRoleId(int roleId);

    Task<bool> AddPermissionForRole(PermissionEntity createPermission);

    Task<bool> RemovePermissionForRole(int permissionId, int roleId);
}