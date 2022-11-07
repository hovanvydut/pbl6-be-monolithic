using Monolithic.Models.ReqParams;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Helpers;

namespace Monolithic.Services.Interface;

public interface IRoleService
{
    Task<bool> CreateRole(CreateRoleDTO createRoleDTO);

    Task<bool> UpdateRole(int roleId, UpdateRoleDTO updateRoleDTO);

    Task<PagedList<RoleDTO>> GetWithParams(RoleParams roleParams);

    Task<List<RoleDTO>> GetAllRoles();

    Task<RoleDTO> GetRoleById(int roleId);

    Task<List<PermissionDTO>> GetPermissionByRoleId(int roleId);

    List<PermissionGroupDTO> GroupPermission(List<PermissionDTO> listPerDTO);

    Task<bool> AddPermissionForRole(CreatePermissionDTO createPermissionDTO);

    Task<bool> RemovePermissionForRole(int permissionId, int roleId);
}