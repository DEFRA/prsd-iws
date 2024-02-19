namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.EditContact
{
    using EA.Iws.Core.Shared;
    using System.ComponentModel.DataAnnotations;

    public class EditContactViewModel
    {
        public EditContactViewModel()
        {
        }

        public EditContactViewModel(dynamic data)
        {
            Name = data.Business.Name;
            FullName = data.Contact.FullName;
            Email = data.Contact.Email;
            TelephonePrefix = data.Contact.TelephonePrefix;
            Telephone = data.Contact.Telephone;
            PostalCode = data.Address.PostalCode;
            AdditionalCharge = new AdditionalChargeData()
            {
                NotificationId = data.NotificationId
            };
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
    }
}