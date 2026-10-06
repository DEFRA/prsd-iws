namespace EA.Iws.Web.Areas.Admin.ViewModels.Menu
{
    using Google.Apis.TagManager.v2.Data;
    using Infrastructure;

    public class AdminLinksViewModel
    {
        public bool ShowApproveNewInternalUserLink { get; set; }

        public bool ShowManageExistingInternalUserLink { get; set; }

        public bool ShowAddNewEntryOrExitPointLink { get; set; }

        public bool ShowDeleteNotificationLink { get; set; }

        public AdminHomeNavigationSection ActiveSection { get; set; }

        public bool ShowManageExternalUserLink { get; set; }

        public bool ShowArchiveNotificationsLink { get; set; }

        public bool ShowNotificationLinks { get; set; }

        public int UsersAwaitingApproval { get; set; }

        public string ManageNewUsersText
        {
            get
            {
                return "Manage new users (" + UsersAwaitingApproval.ToString() + ")";
            }
        }
    }
}