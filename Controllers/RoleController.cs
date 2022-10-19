using static Monolithic.Constants.PermissionPolicy;
using Microsoft.AspNetCore.Authorization;
using Monolithic.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Monolithic.Models.Common;
using Monolithic.Models.DTO;
using Monolithic.Constants;
using Monolithic.Helpers;

namespace Monolithic.Controllers;

public class RoleController : BaseController
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost]
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
    public async Task<BaseResponse<List<RoleDTO>>> GetAllRoles()
    {
        var roles = await _roleService.GetAllRoles();
        return new BaseResponse<List<RoleDTO>>(roles, HttpCode.OK);
    }

    [HttpGet("{roleId}")]
    public async Task<BaseResponse<RoleDTO>> GetRoleById(int roleId)
    {
        var role = await _roleService.GetRoleById(roleId);
        return new BaseResponse<RoleDTO>(role, HttpCode.OK);
    }

    [HttpGet("Permission/{roleId}/Not-Have")]
    public async Task<BaseResponse<List<PermissionContent>>> GetPermissionsRoleNotHave(int roleId)
    {
        var listPermission = await _roleService.GetPermissionsRoleNotHave(roleId);
        return new BaseResponse<List<PermissionContent>>(listPermission, HttpCode.OK);
    }

    [HttpGet("Permission/{roleId}")]
    public async Task<BaseResponse<List<PermissionDTO>>> GetPermissionByRoleId(int roleId)
    {
        var listPermission = await _roleService.GetPermissionByRoleId(roleId);
        return new BaseResponse<List<PermissionDTO>>(listPermission, HttpCode.OK);
    }

    [HttpPost("Permission")]
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

    [HttpDelete("Permission")]
    public async Task<BaseResponse<bool>> RemovePermissionForRole(int permissionId, int roleId)
    {
        var permisisonDeleted = await _roleService.RemovePermissionForRole(permissionId, roleId);
        if (permisisonDeleted)
            return new BaseResponse<bool>(permisisonDeleted, HttpCode.NO_CONTENT);
        else
            return new BaseResponse<bool>(permisisonDeleted, HttpCode.BAD_REQUEST, "", false);
    }
}