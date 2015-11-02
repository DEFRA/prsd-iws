namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.Shared
{
    using System.ComponentModel.DataAnnotations;

    public class ContactViewModel
    {
        [Display(Name = "Name", ResourceType = typeof(ContactViewModelResources))]
        public string Name { get; set; }

        [Display(Name = "Telephone", ResourceType = typeof(ContactViewModelResources))]
        public string Telephone { get; set; }

        [Display(Name = "Email", ResourceType = typeof(ContactViewModelResources))]
        public string Email { get; set; }
    }
}