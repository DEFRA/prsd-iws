namespace EA.Iws.Web.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;
    using Views.Account;

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email", ResourceType = typeof(LoginResources))]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(LoginResources))]
        public string Password { get; set; }
    }
}