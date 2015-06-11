namespace EA.Iws.Web.ViewModels.NotificationApplication
{
    using Requests.Notification;
    using Shared;

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