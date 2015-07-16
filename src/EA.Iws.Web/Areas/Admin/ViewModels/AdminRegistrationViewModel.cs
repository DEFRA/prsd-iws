namespace EA.Iws.Web.Areas.Admin.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using Prsd.Core.Validation;

    public class AdminRegistrationViewModel
    {
        [Required]
        [Display(Name = "First name")]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Text)]
        [Display(Name = "Last name")]
        public string Surname { get; set; }

        [Required]
        [StringLength(80)]
        [DataType(DataType.Text)]
        [Display(Name = "Competent authority")]
        public string CompetentAuthority { get; set; }

        [Required]
        [StringLength(80)]
        [DataType(DataType.Text)]
        [Display(Name = "Local area covered")]
        public string LocalArea { get; set; }

        [Required]
        [StringLength(80)]
        [DataType(DataType.Text)]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}