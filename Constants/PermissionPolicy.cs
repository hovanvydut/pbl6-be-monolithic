using Monolithic.Helpers;

namespace Monolithic.Constants;

public static class PermissionPolicy
{
    public static List<PermissionContent> AllPermissions = new List<PermissionContent>()
    {
        // USER
        new PermissionContent(UserPermission.ViewPersonal,"Can view personal user profile"),
        new PermissionContent(UserPermission.UpdateProfile,"Can update user profile"),
        new PermissionContent(UserPermission.ViewAll,"Can view all users in system"),
        new PermissionContent(UserPermission.UpdateAccountAccess, "Can update user account access in system"),
        new PermissionContent(UserPermission.ViewAccountAccess, "Can view user account access in system"),

        // BOOKMARK
        new PermissionContent(BookmarkPermission.View,"Can view bookmarked post"),
        new PermissionContent(BookmarkPermission.Create,"Can create a bookmark"),
        new PermissionContent(BookmarkPermission.Remove,"Can remove a bookmark"),

        // ROLE
        new PermissionContent(RolePermission.Create,"Can create a role"),
        new PermissionContent(RolePermission.Update,"Can update a role"),
        new PermissionContent(RolePermission.ViewAll,"Can view all role"),
        new PermissionContent(RolePermission.ViewOne,"Can view a role detail"),

        // PERMISSION
        new PermissionContent(PermissionPermission.View, "Can view permission of a role"),
        new PermissionContent(PermissionPermission.Create, "Can add new permission for role"),
        new PermissionContent(PermissionPermission.Remove, "Can remove permission from role"),

        // FREE TIME
        new PermissionContent(FreeTimePermission.ViewAll, "Can view all free time"),
        new PermissionContent(FreeTimePermission.Create, "Can create free time"),

        // BOOKING
        new PermissionContent(BookingPermission.ViewAllPersonal, "Can view all meeting of host"),
        new PermissionContent(BookingPermission.ViewAllBooked, "Can view all meeting of guest"),
        new PermissionContent(BookingPermission.CreateMeeting, "Can create meeting"),
        new PermissionContent(BookingPermission.ApproveMeeting, "Can approve meeting"),
        new PermissionContent(BookingPermission.ConfirmMet, "Can confirm guest met motel"),

        // CONFIG SETTING
        new PermissionContent(ConfigSettingPermission.ViewAll, "Can view all config setting"),
        new PermissionContent(ConfigSettingPermission.ViewOne, "Can view one config setting"),
        new PermissionContent(ConfigSettingPermission.Update, "Can update config setting"),

        // NOTIFICATION
        new PermissionContent(NotificationPermission.ViewAll, "Can view all notification"),
        new PermissionContent(NotificationPermission.Update, "Can update notification"),

        // VNP
        new PermissionContent(VNPPermission.CreatePayment, "Can create vnp payment"),
        new PermissionContent(VNPPermission.ViewAllHistory, "Can view all vnp history in system"),
        new PermissionContent(VNPPermission.ViewAllHistoryPersonal, "Can view personal vnp history"),

        // PAYMENT
        new PermissionContent(PaymentPermission.ViewAllHistory, "Can view all payment history in system"),
        new PermissionContent(PaymentPermission.ViewAllHistoryPersonal, "Can view personal payment history"),

        // POST
        new PermissionContent(PostPermission.ViewAllPersonal, "Can view all personal post"),
        new PermissionContent(PostPermission.Create, "Can create post"),
        new PermissionContent(PostPermission.Update, "Can update post"),
        new PermissionContent(PostPermission.Delete, "Can delete post"),
        // UPTOP
        new PermissionContent(PostPermission.CreateUptop, "Can create uptop post"),
        new PermissionContent(PostPermission.GetUptop, "Can get uptop post"),
        new PermissionContent(PostPermission.CheckDuplicateUptop, "Can check duplicate uptop post"),

        // REVIEW
        new PermissionContent(ReviewPermission.Create, "Can create review"),
        new PermissionContent(ReviewPermission.CheckCanReview, "Can check can review"),

        // POST STATISTIC
        new PermissionContent(PostStatisticPermission.ViewInDateRange, "Can view post statistic in date range"),
        new PermissionContent(PostStatisticPermission.ViewDetailInDate, "Can view post statistic detail in date"),
        new PermissionContent(PostStatisticPermission.ViewTopInDate, "Can view top post statistic in date"),

        // USER STATISTIC
        new PermissionContent(UserStatisticPermission.ViewInDateRange, "Can view user statistic in date range"),
        new PermissionContent(UserStatisticPermission.ViewDetailInDate, "Can view user statistic detail in date"),
        new PermissionContent(UserStatisticPermission.ViewTopInDate, "Can view top user statistic in date"),
    };

    public static class UserPermission
    {
        public const string ViewPersonal = "User.View.Personal";
        public const string UpdateProfile = "User.Update.Profile";
        public const string ViewAll = "User.View.All";
        public const string UpdateAccountAccess = "User.Update.Account.Access";
        public const string ViewAccountAccess = "User.View.Account.Access";
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
        public const string Remove = "Permission.Remove";
    }

    public static class FreeTimePermission
    {
        public const string ViewAll = "FreeTime.View.All";
        public const string Create = "FreeTime.Create";
    }

    public static class BookingPermission
    {
        public const string ViewAllPersonal = "Booking.View.All.Personal";
        public const string ViewAllBooked = "Booking.View.All.Booked";
        public const string CreateMeeting = "Booking.Create.Meeting";
        public const string ApproveMeeting = "Booking.Approve.Meeting";
        public const string ConfirmMet = "Booking.Confirm.Met";
    }

    public static class ConfigSettingPermission
    {
        public const string ViewAll = "ConfigSetting.View.All";
        public const string ViewOne = "ConfigSetting.View.One";
        public const string Update = "ConfigSetting.Update";
    }

    public static class NotificationPermission
    {
        public const string ViewAll = "Notification.View.All";
        public const string Update = "Notification.Update";
    }

    public static class VNPPermission
    {
        public const string CreatePayment = "VNP.Create.Payment";
        public const string ViewAllHistory = "VNP.View.All.History";
        public const string ViewAllHistoryPersonal = "VNP.View.All.History.Personal";
    }

    public static class PaymentPermission
    {
        public const string ViewAllHistory = "Payment.View.All.History";
        public const string ViewAllHistoryPersonal = "Payment.View.All.History.Personal";
    }

    public static class PostPermission
    {
        public const string ViewAllPersonal = "Post.View.All.Personal";
        public const string Create = "Post.Create";
        public const string Update = "Post.Update";
        public const string Delete = "Post.Delete";

        public const string CreateUptop = "Post.Create.Uptop";
        public const string GetUptop = "Post.Get.Uptop";
        public const string CheckDuplicateUptop = "Post.Check.Duplicate.Uptop";
    }

    public static class ReviewPermission
    {
        public const string Create = "Review.Create";
        public const string CheckCanReview = "Review.Check.Can.Review";
    }

    public static class PostStatisticPermission
    {
        public const string ViewInDateRange = "PostStatistic.View.In.Date.Range";
        public const string ViewDetailInDate = "PostStatistic.View.Detail.In.Date";
        public const string ViewTopInDate = "PostStatistic.View.Top.In.Date";
    }

    public static class UserStatisticPermission
    {
        public const string ViewInDateRange = "UserStatistic.View.In.Date.Range";
        public const string ViewDetailInDate = "UserStatistic.View.Detail.In.Date";
        public const string ViewTopInDate = "UserStatistic.View.Top.In.Date";
    }
}