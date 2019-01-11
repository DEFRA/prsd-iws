namespace EA.Iws.Core.Authorization.Permissions
{
    public static class GeneralPermissions
    {
        public const string CanAuthorizeActivity = "Can Authorize Activity";

        public const string CanReadCountryData = "Can Read Country Data";

        public const string CanReadUserData = "Can Read User Data";

        public const string CanEditUserData = "Can Edit User Data";

        public const string CanReadOrganisationData = "Can Read Organisation Data";

        public const string CanEditOrganisationData = "Can Edit Organisation Data";

        public const string CanReadAddressBook = "Can Read Address Book";

        public const string CanEditAddressBook = "Can Edit Address Book";

        public const string CanViewSearchResults = "Can View Search Results";

        public const string CanReadImportExportNotificationData = "Can Read Import Export Notification Data";

        //COULLM: Is this the right location for the new permission? Is there an existing permission that should be used instead?
        public const string CanViewNotificationUpdateHistory = "Can View Notification Update History";
    }
}