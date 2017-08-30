namespace EA.Iws.Web.Areas.Admin.ViewModels.ExternalUserAdministration
{
    using System.ComponentModel.DataAnnotations;

    public class DeactivateUserViewModel
    {
        [EmailAddress(ErrorMessageResourceName = "EmailValid", ErrorMessageResourceType = typeof(DeactivateUserViewModelResources))]
        [Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(DeactivateUserViewModelResources))]
        [Display(Name = "Email", ResourceType = typeof(DeactivateUserViewModelResources))]
        public string Email { get; set; }
    }
}