namespace EA.Iws.Web.Areas.Admin.ViewModels.ExportNotification
{
    using Core.Notification;
    using Web.ViewModels.Shared;

    public class CompetentAuthorityChoiceViewModel
    {
        public CompetentAuthorityChoiceViewModel()
        {
            CompetentAuthorities =
                RadioButtonStringCollectionViewModel.CreateFromEnum<CompetentAuthority>();
        }

        public RadioButtonStringCollectionViewModel CompetentAuthorities { get; set; }
    }
}