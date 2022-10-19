using Monolithic.Helpers;

namespace Monolithic.Constants;

public static class PermissionPolicy
{
    public static List<PermissionContent> AllPermissions = new List<PermissionContent>()
    {
        new PermissionContent(EmailPermission.Send,"Can send mail"),
        new PermissionContent(UserPermission.ViewPersonal,"Can view personal user profile"),
        new PermissionContent(UserPermission.ViewAnonymous,"Can view personal user profile"),
        new PermissionContent(UserPermission.UpdateProfile,"Can update user profile"),
    };

    public static class EmailPermission
    {
        public const string Send = "Email.Send";
    }

    public static class UserPermission
    {
        public const string ViewPersonal = "User.View.Personal";
        public const string ViewAnonymous = "User.View.Anonymous";
        public const string UpdateProfile = "User.Update.Profile";
    }
}