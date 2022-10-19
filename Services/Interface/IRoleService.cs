using Monolithic.Models.DTO;
using Monolithic.Helpers;

namespace Monolithic.Services.Interface;

public interface IRoleService
{
    Task<bool> CreateRole(CreateRoleDTO createRoleDTO);

    Task<bool> UpdateRole(int roleId, UpdateRoleDTO updateRoleDTO);

    Task<RoleDTO> GetRoleById(int roleId);

    Task<List<PermissionContent>> GetPermissionsRoleNotHave(int roleId);

    Task<List<PermissionDTO>> GetPermissionByRoleId(int roleId);

    Task<bool> AddPermissionForRole(CreatePermissionDTO createPermissionDTO);

    Task<bool> RemovePermissionForRole(int permissionId, int roleId);
}