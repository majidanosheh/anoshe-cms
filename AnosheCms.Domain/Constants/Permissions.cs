// File: AnosheCms.Domain/Constants/Permissions.cs
// (تصحیح شد: وابستگی به Application و متد GetAllPermissionGroups حذف شد)

namespace AnosheCms.Domain.Constants
{
    // (این کلاس اکنون فقط ثابت‌ها را نگه می‌دارد)
    public static class Permissions
    {
        public const string ViewDashboard = "Permissions.Dashboard.View";

        public const string ViewContentTypes = "Permissions.ContentTypes.View";
        public const string CreateContentTypes = "Permissions.ContentTypes.Create";
        public const string EditContentTypes = "Permissions.ContentTypes.Edit";
        public const string DeleteContentTypes = "Permissions.ContentTypes.Delete";

        public const string ViewMedia = "Permissions.Media.View";
        public const string CreateMedia = "Permissions.Media.Create";
        public const string DeleteMedia = "Permissions.Media.Delete";

        public const string ViewSettings = "Permissions.Settings.View";
        public const string EditSettings = "Permissions.Settings.Edit";

        public const string ViewContent = "Permissions.Content.View";
        public const string CreateContent = "Permissions.Content.Create";
        public const string EditContent = "Permissions.Content.Edit";
        public const string DeleteContent = "Permissions.Content.Delete";

        public const string ViewUsers = "Permissions.Users.View";
        public const string CreateUsers = "Permissions.Users.Create";
        public const string EditUsers = "Permissions.Users.Edit";
        public const string DeleteUsers = "Permissions.Users.Delete";

        public const string ViewRoles = "Permissions.Roles.View";
        public const string CreateRoles = "Permissions.Roles.Create";
        public const string EditRoles = "Permissions.Roles.Edit";
        public const string DeleteRoles = "Permissions.Roles.Delete";
        public const string ManagePermissions = "Permissions.Roles.ManagePermissions";
    }
}