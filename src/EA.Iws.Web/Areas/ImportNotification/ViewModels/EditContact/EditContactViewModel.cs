namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.EditContact
{
    using EA.Iws.Core.ImportNotificationAssessment;
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.Shared;
    using System.ComponentModel.DataAnnotations;

    public class EditContactViewModel
    {
        public EditContactViewModel()
        {
        }

        public EditContactViewModel(dynamic data)
        {
            Name = data.Name;
            FullName = data.Contact.Name;
            Email = data.Contact.Email;
            TelephonePrefix = data.Contact.TelephonePrefix;
            Telephone = data.Contact.Telephone;
            PostalCode = data.Address.PostalCode;
            AdditionalCharge = new AdditionalChargeData()
            {
                NotificationId = data.NotificationId
            };
            CompetentAuthority = data.CompetentAuthority;
            NotificationStatus = data.NotificationStatus;
            ShowAdditionalCharge = ((data.CompetentAuthority == UKCompetentAuthority.England || data.CompetentAuthority == UKCompetentAuthority.Scotland) &&
                                    ((data.NotificationStatus == ImportNotificationStatus.Consented) ||
                                    (data.NotificationStatus == ImportNotificationStatus.DecisionRequiredBy))) ? true : false;
        }

        [Required(ErrorMessageResourceType = typeof(EditContactViewModelResources), ErrorMessageResourceName = "OrgNameRequired")]
        [Display(Name = "OrgName", ResourceType = typeof(EditContactViewModelResources))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(EditContactViewModelResources), ErrorMessageResourceName = "FullNameRequired")]
        [Display(Name = "FullName", ResourceType = typeof(EditContactViewModelResources))]
        public string FullName { get; set; }

        [Required(ErrorMessageResourceType = typeof(EditContactViewModelResources), ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(EditContactViewModelResources))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(EditContactViewModelResources), ErrorMessageResourceName = "TelephonePrefixRequired")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "TelephonePrefix", ResourceType = typeof(EditContactViewModelResources))]
        [StringLength(3)]
        [RegularExpression("\\d+", ErrorMessageResourceType = typeof(EditContactViewModelResources), ErrorMessageResourceName = "TelephoneInvalid")]
        public string TelephonePrefix { get; set; }

        [Required(ErrorMessageResourceType = typeof(EditContactViewModelResources), ErrorMessageResourceName = "TelephoneNumberRequired")]
        [Display(Name = "TelephoneNumber", ResourceType = typeof(EditContactViewModelResources))]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"[\d]+[\d\s]+[\d]+", ErrorMessageResourceType = typeof(EditContactViewModelResources), ErrorMessageResourceName = "TelephoneInvalid")]
        public string Telephone { get; set; }

        [Required(ErrorMessageResourceType = typeof(EditContactViewModelResources), ErrorMessageResourceName = "PostalcodeRequired")]
        [Display(Name = "PostCode", ResourceType = typeof(EditContactViewModelResources))]
        public string PostalCode { get; set; }

        public AdditionalChargeData AdditionalCharge { get; set; }

        public UKCompetentAuthority CompetentAuthority { get; set; }

        public bool ShowAdditionalCharge { get; set; }

        public ImportNotificationStatus NotificationStatus { get; set; }
    }
}