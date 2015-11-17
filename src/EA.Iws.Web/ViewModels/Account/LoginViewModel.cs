namespace EA.Iws.Web.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;
    using Views.Account;

    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(LoginResources), ErrorMessageResourceName = "EmailRequired")]
        [Display(Name = "Email", ResourceType = typeof(LoginResources))]
        [EmailAddress(ErrorMessageResourceType = typeof(LoginResources), ErrorMessageResourceName = "EmailFormatValidation")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(LoginResources), ErrorMessageResourceName = "PasswordRequired")]
        [DataType(DataType.Password, ErrorMessageResourceType = typeof(LoginResources), ErrorMessageResourceName = "PasswordFormatValidation")]
        [Display(Name = "Password", ResourceType = typeof(LoginResources))]
        public string Password { get; set; }
    }
}