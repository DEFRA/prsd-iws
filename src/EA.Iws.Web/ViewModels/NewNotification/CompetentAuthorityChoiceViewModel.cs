namespace EA.Iws.Web.ViewModels.NewNotification
{
    using Core.Notification;
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