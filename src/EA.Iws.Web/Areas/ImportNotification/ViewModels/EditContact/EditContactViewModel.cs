namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.EditContact
{
    using System.ComponentModel.DataAnnotations;
    using Core.ImportNotification.Summary;
    public class EditContactViewModel
    {
        public EditContactViewModel()
        {
        }

        public EditContactViewModel(Exporter data)
        {
            FullName = data.Contact.Name;
            Email = data.Contact.Email;
            TelephonePrefix = data.Contact.TelephonePrefix;
            Telephone = data.Contact.Telephone;
        }

        public EditContactViewModel(Importer data)
        {
            FullName = data.Contact.Name;
            Email = data.Contact.Email;
            TelephonePrefix = data.Contact.TelephonePrefix;
            Telephone = data.Contact.Telephone;
        }

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
    }
}