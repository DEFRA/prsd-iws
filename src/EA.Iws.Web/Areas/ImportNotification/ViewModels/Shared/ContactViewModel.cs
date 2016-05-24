namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.Shared
{
    using System.ComponentModel.DataAnnotations;
    using Core.ImportNotification.Draft;

    public class ContactViewModel
    {
        [Display(Name = "Name", ResourceType = typeof(ContactViewModelResources))]
        public string Name { get; set; }
        
        [Display(Name = "Telephone", ResourceType = typeof(ContactViewModelResources))]
        [RegularExpression(@"^[+]?[\d]+(( |-)?[\d]+)+?$", ErrorMessageResourceType = typeof(ContactViewModelResources), ErrorMessageResourceName = "TelephoneInvalid")]
        public string Telephone { get; set; }

        [EmailAddress(ErrorMessageResourceType = typeof(ContactViewModelResources), ErrorMessageResourceName = "EmailInvalid", ErrorMessage = null)]
        [Display(Name = "Email", ResourceType = typeof(ContactViewModelResources))]
        public string Email { get; set; }

        public ContactViewModel()
        {
        }

        public ContactViewModel(Contact contact)
        {
            if (contact != null)
            {
                Name = contact.ContactName;
                Telephone = contact.Telephone;
                Email = contact.Email;
            }
        }

        public Contact AsContact()
        {
            return new Contact
            {
                ContactName = Name,
                Email = Email,
                Telephone = Telephone
            };
        }
    }
}