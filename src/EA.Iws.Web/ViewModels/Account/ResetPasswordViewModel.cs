﻿namespace EA.Iws.Web.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;
    using Views.Account;

    public class ResetPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(ResetPasswordResources))]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(ResetPasswordResources))]
        [Compare("Password", ErrorMessageResourceName = "PasswordsDoNotMatch", ErrorMessageResourceType = typeof(ResetPasswordResources))]
        public string ConfirmPassword { get; set; }
    }
}