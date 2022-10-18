namespace EA.Iws.Web.ViewModels.NewNotification
{
    using Core.Notification;
    using DocumentFormat.OpenXml.Wordprocessing;
    using EA.Iws.Core.Shared;
    using EA.Iws.Web.Areas.AddressBook.Views.Edit;
    using EA.Iws.Web.Areas.NotificationApplication.Views.Producer;
    using EA.Iws.Web.Views.NewNotification;
    using Shared;
    using System.ComponentModel.DataAnnotations;
    using System.Security.AccessControl;

    public class CompetentAuthorityChoiceViewModel
    {
        public CompetentAuthorityChoiceViewModel()
        {
        }

        [Required(ErrorMessageResourceName = "CompetentAuthorityRequired", ErrorMessageResourceType = typeof(RadioButtonsResources))]
        [Display(Name = "Title", ResourceType = typeof(CompetentAuthorityResources))]
        public UKCompetentAuthority? AuthorityType { get; set; }
    }
}