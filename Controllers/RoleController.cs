using static Monolithic.Constants.PermissionPolicy;
using Microsoft.AspNetCore.Authorization;
using Monolithic.Services.Interface;
using Monolithic.Models.ReqParams;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Constants;

namespace Monolithic.Controllers;

public class RoleController : BaseController
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost]
    [Authorize(Roles = RolePermission.Create)]
    public async Task<BaseResponse<bool>> CreateRole(CreateRoleDTO createRoleDTO)
    {
        if (ModelState.IsValid)
        {
            var roleCreated = await _roleService.CreateRole(createRoleDTO);
            if (roleCreated)
                return new BaseResponse<bool>(roleCreated, HttpCode.CREATED);
            else
                return new BaseResponse<bool>(roleCreated, HttpCode.BAD_REQUEST, "", false);
        }
        return new BaseResponse<bool>(false, HttpCode.BAD_REQUEST, "", false);
    }

    [HttpPut("{roleId}")]
    [Authorize(Roles = RolePermission.Update)]
    public async Task<BaseResponse<bool>> UpdateRole(int roleId, UpdateRoleDTO updateRoleDTO)
    {
        if (ModelState.IsValid)
        {
            var roleUpdated = await _roleService.UpdateRole(roleId, updateRoleDTO);
            if (roleUpdated)
                return new BaseResponse<bool>(roleUpdated, HttpCode.NO_CONTENT);
            else
                return new BaseResponse<bool>(roleUpdated, HttpCode.BAD_REQUEST, "", false);
        }
        return new BaseResponse<bool>(false, HttpCode.BAD_REQUEST, "", false);
    }

    [HttpGet]
    [Authorize(Roles = RolePermission.ViewAll)]
    public async Task<BaseResponse<PagedList<RoleDTO>>> GetWithParams([FromQuery] RoleParams roleParams)
    {
        var roles = await _roleService.GetWithParams(roleParams);
        return new BaseResponse<PagedList<RoleDTO>>(roles);
    }

    [HttpGet("all")]
    [Authorize(Roles = RolePermission.ViewAll)]
    public async Task<BaseResponse<List<RoleDTO>>> GetAllRoles()
    {
        var roles = await _roleService.GetAllRoles();
        return new BaseResponse<List<RoleDTO>>(roles);
    }

    [HttpGet("{roleId}")]
    [Authorize(Roles = RolePermission.ViewOne)]
    public async Task<BaseResponse<RoleDTO>> GetRoleById(int roleId)
    {
        var role = await _roleService.GetRoleById(roleId);
        return new BaseResponse<RoleDTO>(role);
    }

    [HttpGet("{roleId}/Permission")]
    [Authorize(Roles = PermissionPermission.View)]
    public async Task<BaseResponse<List<PermissionGroupDTO>>> GetPermissionByRoleId(int roleId)
    {
        // permission not have => id = 0
        // permission have => id > 0
        var listPermission = await _roleService.GetPermissionByRoleId(roleId);
        var listPermissionGroup = _roleService.GroupPermission(listPermission);
        return new BaseResponse<List<PermissionGroupDTO>>(listPermissionGroup);
    }

    [HttpPost("Permission")]
    [Authorize(Roles = PermissionPermission.Create)]
    public async Task<BaseResponse<bool>> AddPermissionForRole(CreatePermissionDTO createPermissionDTO)
    {
        if (ModelState.IsValid)
        {
            var permisisonCreated = await _roleService.AddPermissionForRole(createPermissionDTO);
            if (permisisonCreated)
                return new BaseResponse<bool>(permisisonCreated, HttpCode.CREATED);
            else
                return new BaseResponse<bool>(permisisonCreated, HttpCode.BAD_REQUEST, "", false);
        }
        return new BaseResponse<bool>(false, HttpCode.BAD_REQUEST, "", false);
    }

    [HttpDelete("{roleId}/Permission/{permissionId}")]
    [Authorize(Roles = PermissionPermission.Remove)]
    public async Task<BaseResponse<bool>> RemovePermissionForRole(int permissionId, int roleId)
    {
        var permisisonDeleted = await _roleService.RemovePermissionForRole(permissionId, roleId);
        if (permisisonDeleted)
            return new BaseResponse<bool>(permisisonDeleted, HttpCode.NO_CONTENT);
        else
            return new BaseResponse<bool>(permisisonDeleted, HttpCode.BAD_REQUEST, "", false);
    }
}