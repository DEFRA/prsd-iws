namespace EA.Iws.Web.Areas.Admin.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Core.Notification;
    using Infrastructure.Validation;

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
        [Display(Name = "Competent authority")]
        public CompetentAuthority? CompetentAuthority { get; set; }

        [Required]
        [Display(Name = "Local area covered")]
        public Guid? LocalAreaId { get; set; }

        [Required]
        [StringLength(80)]
        [DataType(DataType.Text)]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [CompetentAuthorityEmailAddress("CompetentAuthority")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password",
            ErrorMessage = "The password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }

        public SelectList Areas { get; set; }

        public SelectList CompetentAuthorities { get; set; }
    }
}