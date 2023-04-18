namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.Shared
{
    using System.ComponentModel.DataAnnotations;
    using Core.ImportNotification.Draft;

    public class ContactViewModel
    {
        [Display(Name = "Name", ResourceType = typeof(ContactViewModelResources))]
        public string FullName { get; set; }
        
        [Display(Name = "Telephone", ResourceType = typeof(ContactViewModelResources))]
        [RegularExpression(@"^[+]?[\d]+(( |-)?[\d]+)+?$", ErrorMessageResourceType = typeof(ContactViewModelResources), ErrorMessageResourceName = "TelephoneInvalid")]
        public string Telephone { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "TelephonePrefix", ResourceType = typeof(ContactViewModelResources))]
        [RegularExpression(@"(\b\d{1,3}\b)", ErrorMessageResourceType = typeof(ContactViewModelResources), ErrorMessageResourceName = "TelephonePrefixInvalid")]
        public string TelephonePrefix { get; set; }

        [EmailAddress(ErrorMessageResourceType = typeof(ContactViewModelResources), ErrorMessageResourceName = "EmailInvalid", ErrorMessage = null)]
        [Display(Name = "Email", ResourceType = typeof(ContactViewModelResources))]
        public string Email { get; set; }

        public AddressTypeEnum AddressType { get; set; }

        public ContactViewModel()
        {
        }

        public ContactViewModel(AddressTypeEnum addressType)
        {
            this.AddressType = addressType;
        }

        public ContactViewModel(Contact contact, AddressTypeEnum addressType)
        {
            if (contact != null)
            {
                FullName = contact.ContactName;
                Telephone = contact.Telephone;
                Email = contact.Email;
                TelephonePrefix = contact.TelephonePrefix;
            }
            AddressType = addressType;
        }

        public Contact AsContact()
        {
            return new Contact
            {
                ContactName = FullName,
                Email = Email,
                Telephone = Telephone,
                TelephonePrefix = TelephonePrefix
            };
        }
    }
}