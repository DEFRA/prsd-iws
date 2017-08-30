namespace EA.Iws.Web.Areas.Admin.ViewModels.Menu
{
    using Infrastructure;

    public class AdminLinksViewModel
    {
        public bool ShowApproveNewInternalUserLink { get; set; }

        public bool ShowManageExistingInternalUserLink { get; set; }

        public bool ShowAddNewEntryOrExitPointLink { get; set; }

        public bool ShowDeleteNotificationLink { get; set; }

        public AdminHomeNavigationSection ActiveSection { get; set; }

        public bool ShowManageExternalUserLink { get; set; }

        public bool ShowNotificationLinks { get; set; }
    }
}