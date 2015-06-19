namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using Requests.Notification;
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