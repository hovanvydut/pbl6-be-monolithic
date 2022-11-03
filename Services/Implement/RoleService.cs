using Monolithic.Repositories.Interface;
using Monolithic.Services.Interface;
using Monolithic.Models.ReqParams;
using Monolithic.Models.Entities;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Constants;
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

    public async Task<PagedList<RoleDTO>> GetAllRoles(RoleParams roleParams)
    {
        var listRole = await _roleRepo.GetAllRoles(roleParams);
        var listRoleDTO = listRole.Records.Select(role => new RoleDTO()
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            Permissions = new List<PermissionDTO>()
        }).ToList();
        return new PagedList<RoleDTO>(listRoleDTO, listRole.TotalRecords);
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

    public async Task<List<PermissionDTO>> GetPermissionByRoleId(int roleId)
    {
        var allPermission = PermissionPolicy.AllPermissions;
        var permissionHave = await _roleRepo.GetPermissionByRoleId(roleId);
        var permissionHaveKey = permissionHave.Select(c => c.Key);

        var permissionNotHaveDTO = allPermission.Where(c => !permissionHaveKey.Contains(c.Key))
                .Select(c => new PermissionDTO()
                {
                    Id = 0,
                    Key = c.Key,
                    Description = c.Description,
                }).ToList();
        var permissionHaveDTO = permissionHave.Select(c => _mapper.Map<PermissionDTO>(c)).ToList();
        return permissionNotHaveDTO.Concat(permissionHaveDTO).ToList();
    }

    public List<PermissionGroupDTO> GroupPermission(List<PermissionDTO> listPerDTO)
    {
        return listPerDTO.GroupBy(c => c.Key.Split(".")[0])
                .Select(c => new PermissionGroupDTO()
                {
                    Key = c.Key,
                    Children = c.Select(d => new PermissionDTO()
                    {
                        Id = d.Id,
                        Key = d.Key,
                        Description = d.Description
                    }).OrderBy(e => e.Key).ToList()
                }).OrderBy(c => c.Key).ToList();
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