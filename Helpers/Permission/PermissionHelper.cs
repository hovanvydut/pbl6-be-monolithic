using static Monolithic.Constants.PermissionPolicy;
using System.Reflection;

namespace Monolithic.Helpers;

public class PermissionHelper : IPermissionHelper
{
    public List<PermissionContent> GetAllPermission()
    {
        List<PermissionContent> listPermission = new List<PermissionContent>();
        listPermission.AddRange(GetPermissionForModule(typeof(Example1)));
        listPermission.AddRange(GetPermissionForModule(typeof(Example2)));
        return listPermission;
    }

    private List<PermissionContent> GetPermissionForModule(Type type)
    {
        return GetAllPublicConstantValues<string>(type)
                    .Select(c => new PermissionContent
                    {
                        Key = c,
                        Description = GetDescFromKey(c)
                    }).ToList();
    }

    private List<T> GetAllPublicConstantValues<T>(Type type)
    {
        return type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                   .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(T))
                   .Select(x => (T)x.GetRawConstantValue())
                   .ToList();
    }

    private string GetDescFromKey(string key)
    {
        //"key": "Permission.Dashboard.View" => "description": "Can View Dashboard"
        var keyArr = key.Split(".").Skip(1).Append("Can").Reverse();
        return string.Join(" ", keyArr);
    }
}