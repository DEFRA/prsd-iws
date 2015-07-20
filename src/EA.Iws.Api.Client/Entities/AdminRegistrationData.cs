namespace EA.Iws.Api.Client.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class AdminRegistrationData
    {
        [Required]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string JobTitle { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string LocalArea { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string CompetentAuthority { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}