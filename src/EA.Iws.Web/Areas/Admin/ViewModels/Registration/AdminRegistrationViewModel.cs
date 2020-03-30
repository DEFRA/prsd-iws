namespace EA.Iws.Web.Areas.Admin.ViewModels.Registration
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Core.Notification;
    using Infrastructure.Validation;
    using Web.ViewModels.Registration;

    public class AdminRegistrationViewModel : CreateUserModelBase
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
        public UKCompetentAuthority? CompetentAuthority { get; set; }

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
        public override string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public override string Password { get; set; }

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