namespace EA.Iws.Web.Areas.Admin.ViewModels.ExternalUserAdministration
{
    using System.ComponentModel.DataAnnotations;

    public class ExternalUserAdministrationViewModel
    {
        [EmailAddress(ErrorMessageResourceName = "EmailValid", ErrorMessageResourceType = typeof(ExternalUserAdministrationViewModelResources), ErrorMessage = null)]
        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(ExternalUserAdministrationViewModelResources))]
        [Display(Name = "Email", ResourceType = typeof(ExternalUserAdministrationViewModelResources))]
        public string Email { get; set; }
    }
}