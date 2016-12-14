namespace EA.Iws.Web.Areas.Admin.ViewModels.Menu
{
    using Infrastructure;

    public class AdminLinksViewModel
    {
        public bool ShowApproveNewInternalUserLink { get; set; }

        public bool ShowManageExistingInternalUserLink { get; set; }

        public bool ShowAddNewEntryOrExitPointLink { get; set; }

        public AdminHomeNavigationSection ActiveSection { get; set; }
    }
}