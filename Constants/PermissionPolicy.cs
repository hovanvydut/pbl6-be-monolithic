namespace Monolithic.Constants;

public static class PermissionPolicy
{
    public static Dictionary<string, string> AllPermissions = new Dictionary<string, string>()
    {
        [EmailSend] = "Can send mail",
        [UserViewPersonal] = "Can view personal user profile",
        [UserViewAnonymous] = "Can view personal user profile",
        [UserUpdateProfile] = "Can update user profile",
    };

    // Mail
    public const string EmailSend = "Email.Send";

    // User
    public const string UserViewPersonal = "User.View.Personal";
    public const string UserViewAnonymous = "User.View.Anonymous";
    public const string UserUpdateProfile = "User.Update.Profile";
}