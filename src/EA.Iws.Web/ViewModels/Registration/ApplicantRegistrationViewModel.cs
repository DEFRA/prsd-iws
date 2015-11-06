namespace EA.Iws.Web.ViewModels.Registration
{
    using System.ComponentModel.DataAnnotations;
    using Core.Shared;
    using Prsd.Core.Validation;
    using Views.Registration;

    public class ApplicantRegistrationViewModel
    {
        public ApplicantRegistrationViewModel()
        {
            Address = new AddressData();
        }

        [Required]
        [Display(Name = "FirstName", ResourceType = typeof(ApplicantRegistrationResources))]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Text)]
        [Display(Name = "LastName", ResourceType = typeof(ApplicantRegistrationResources))]
        public string Surname { get; set; }

        [Required]
        [StringLength(80)]
        [DataType(DataType.Text)]
        [Display(Name = "OrganisationName", ResourceType = typeof(ApplicantRegistrationResources))]
        public string OrganisationName { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "TelephoneNumber", ResourceType = typeof(ApplicantRegistrationResources))]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(ApplicantRegistrationResources))]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceName = "PasswordLength", ErrorMessageResourceType = typeof(ApplicantRegistrationResources), MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(ApplicantRegistrationResources))]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(ApplicantRegistrationResources))]
        [Compare("Password", ErrorMessageResourceName = "PasswordsDoNotMatch", ErrorMessageResourceType = typeof(ApplicantRegistrationResources))]
        public string ConfirmPassword { get; set; }

        [MustBeTrue(ErrorMessageResourceName = "ConfirmTnCs", ErrorMessageResourceType = typeof(ApplicantRegistrationResources))]
        public bool TermsAndConditions { get; set; }

        public AddressData Address { get; set; }
    }
}