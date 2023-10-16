namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.EditContact
{
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
        [Display(Name = "Post Code")]
        public string PostalCode { get; set; }
    }
}