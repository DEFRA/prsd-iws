namespace EA.Iws.Web.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;
    using Views.Account;

    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "Email", ResourceType = typeof(ForgotPasswordResources))]
        [EmailAddress(ErrorMessageResourceName = "EmailInvalid", ErrorMessageResourceType = typeof(ForgotPasswordResources))]
        public string Email { get; set; }
    }
}