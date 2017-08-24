namespace EA.Iws.Web.ViewModels.Registration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Shared;
    using Prsd.Core.Validation;
    using Services;
    using Views.Registration;

    public class ApplicantRegistrationViewModel : IValidatableObject
    {
        public ApplicantRegistrationViewModel()
        {
            Address = new AddressData();
        }

        [Required(ErrorMessageResourceType = typeof(ApplicantRegistrationResources), ErrorMessageResourceName = "FirstNameRequired")]
        [Display(Name = "FirstName", ResourceType = typeof(ApplicantRegistrationResources))]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(ApplicantRegistrationResources), ErrorMessageResourceName = "LastNameRequired")]
        [StringLength(50)]
        [DataType(DataType.Text)]
        [Display(Name = "LastName", ResourceType = typeof(ApplicantRegistrationResources))]
        public string Surname { get; set; }

        [Required(ErrorMessageResourceType = typeof(ApplicantRegistrationResources), ErrorMessageResourceName = "OrganisationNameRequired")]
        [StringLength(80)]
        [DataType(DataType.Text)]
        [Display(Name = "OrganisationName", ResourceType = typeof(ApplicantRegistrationResources))]
        public string OrganisationName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ApplicantRegistrationResources), ErrorMessageResourceName = "TelephoneNumberRequired")]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceType = typeof(ApplicantRegistrationResources), ErrorMessageResourceName = "TelephoneFormatValidation")]
        [Display(Name = "TelephoneNumber", ResourceType = typeof(ApplicantRegistrationResources))]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessageResourceType = typeof(ApplicantRegistrationResources), ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress(ErrorMessageResourceType = typeof(ApplicantRegistrationResources), ErrorMessageResourceName = "EmailFormatValidation", ErrorMessage = null)]
        [Display(Name = "Email", ResourceType = typeof(ApplicantRegistrationResources))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(ApplicantRegistrationResources), ErrorMessageResourceName = "PasswordRequired")]
        [StringLength(100, ErrorMessageResourceName = "PasswordLength", ErrorMessageResourceType = typeof(ApplicantRegistrationResources), MinimumLength = 8)]
        [DataType(DataType.Password, ErrorMessageResourceType = typeof(ApplicantRegistrationResources), ErrorMessageResourceName = "PasswordFormatValidation")]
        [Display(Name = "Password", ResourceType = typeof(ApplicantRegistrationResources))]
        public string Password { get; set; }

        [Required(ErrorMessageResourceType = typeof(ApplicantRegistrationResources), ErrorMessageResourceName = "ConfirmPasswordRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(ApplicantRegistrationResources))]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceName = "PasswordsDoNotMatch", ErrorMessageResourceType = typeof(ApplicantRegistrationResources))]
        public string ConfirmPassword { get; set; }

        [MustBeTrue(ErrorMessageResourceName = "ConfirmTnCs", ErrorMessageResourceType = typeof(ApplicantRegistrationResources))]
        public bool TermsAndConditions { get; set; }

        public AddressData Address { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var config = DependencyResolver.Current.GetService<AppConfiguration>();

            if (config.Environment.Equals("LIVE", StringComparison.InvariantCultureIgnoreCase))
            {
                var competentAuthorityEmailDomains = CompetentAuthorityMetadata.GetValidEmailAddressDomains();
                if (competentAuthorityEmailDomains.Any(domain => Email.ToString().EndsWith(domain)))
                {
                    yield return new ValidationResult(ApplicantRegistrationResources.EmailNotCompetentAuthority, new[] { "Email" });
                }
            }
        }
    }
}