namespace EA.Iws.Web.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;

    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "Email address")]
        [EmailAddress(ErrorMessage = "The email address is not valid.")]
        public string Email { get; set; }
    }
}