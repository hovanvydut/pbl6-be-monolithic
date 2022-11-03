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
        new PermissionContent(UserPermission.ViewAll,"Can view all users in system"),

        new PermissionContent(BookmarkPermission.View,"Can view bookmarked post"),
        new PermissionContent(BookmarkPermission.Create,"Can create a bookmark"),
        new PermissionContent(BookmarkPermission.Remove,"Can remove a bookmark"),

        new PermissionContent(RolePermission.Create,"Can create a role"),
        new PermissionContent(RolePermission.Update,"Can update a role"),
        new PermissionContent(RolePermission.ViewAll,"Can view all role"),
        new PermissionContent(RolePermission.ViewOne,"Can view a role detail"),

        new PermissionContent(PermissionPermission.View,"Can view permission of a role"),
        new PermissionContent(PermissionPermission.Create,"Can add new permission for role"),
        new PermissionContent(PermissionPermission.Remove,"Can remove permission from role"),
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
        public const string ViewAll = "User.View.All";
    }

    public static class BookmarkPermission
    {
        public const string View = "Bookmark.View";
        public const string Create = "Bookmark.Create";
        public const string Remove = "Bookmark.Remove";
    }

    public static class RolePermission
    {
        public const string Create = "Role.Create";
        public const string Update = "Role.Update";
        public const string ViewAll = "Role.View.All";
        public const string ViewOne = "Role.View.One";
    }

    public static class PermissionPermission
    {
        public const string View = "Permission.View";
        public const string Create = "Permission.Create";
        public const string Remove = "Permission.View.Remove";
    }
}