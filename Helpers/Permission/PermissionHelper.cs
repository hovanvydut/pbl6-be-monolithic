using Monolithic.Constants;

namespace Monolithic.Helpers;

public class PermissionHelper : IPermissionHelper
{
    public List<PermissionContent> GetAllPermission()
    {
        var listPermission = PermissionPolicy.AllPermissions.Select(c => new PermissionContent()
        {
            Key = c.Key,
            Description = c.Value
        }).ToList();
        return listPermission;
    }
}