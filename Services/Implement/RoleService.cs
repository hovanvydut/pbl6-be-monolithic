using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;
using Monolithic.Models.Entities;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Constants;
using Monolithic.Helpers;
using AutoMapper;

namespace Monolithic.Services.Implement;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepo;
    private readonly IMapper _mapper;
    public RoleService(IRoleRepository roleRepo,
                       IMapper mapper)
    {
        _roleRepo = roleRepo;
        _mapper = mapper;
    }

    public async Task<bool> CreateRole(CreateRoleDTO createRoleDTO)
    {
        RoleEntity createRole = _mapper.Map<RoleEntity>(createRoleDTO);
        return await _roleRepo.CreateRole(createRole);
    }

    public async Task<bool> UpdateRole(int roleId, UpdateRoleDTO updateRoleDTO)
    {
        RoleEntity updateRole = _mapper.Map<RoleEntity>(updateRoleDTO);
        return await _roleRepo.UpdateRole(roleId, updateRole);
    }

    public async Task<List<RoleDTO>> GetAllRoles()
    {
        var listRole = await _roleRepo.GetAllRoles();
        return listRole.Select(role => new RoleDTO()
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            Permissions = new List<PermissionDTO>()
        }).ToList();
    }

    public async Task<RoleDTO> GetRoleById(int roleId)
    {
        RoleEntity role = await _roleRepo.GetRoleById(roleId);
        if (role == null)
            throw new BaseException(HttpCode.NOT_FOUND, "This role is not found");
        return new RoleDTO()
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            Permissions = role.Permissions.Select(c => _mapper.Map<PermissionDTO>(c)).ToList(),
        };
    }

    public async Task<List<PermissionContent>> GetPermissionsRoleNotHave(int roleId)
    {
        var allPermission = PermissionPolicy.AllPermissions;
        var rolePermission = (await _roleRepo.GetPermissionByRoleId(roleId))
                                .Select(c => c.Key);
        return allPermission.Where(c => !rolePermission.Contains(c.Key)).ToList();
    }

    public async Task<List<PermissionDTO>> GetPermissionByRoleId(int roleId)
    {
        List<PermissionEntity> listPermission = await _roleRepo.GetPermissionByRoleId(roleId);
        return listPermission.Select(c => _mapper.Map<PermissionDTO>(c)).ToList();
    }

    public async Task<bool> AddPermissionForRole(CreatePermissionDTO createPermissionDTO)
    {
        PermissionEntity createPermission = _mapper.Map<PermissionEntity>(createPermissionDTO);
        return await _roleRepo.AddPermissionForRole(createPermission);
    }

    public async Task<bool> RemovePermissionForRole(int permissionId, int roleId)
    {
        return await _roleRepo.RemovePermissionForRole(permissionId, roleId);
    }
}