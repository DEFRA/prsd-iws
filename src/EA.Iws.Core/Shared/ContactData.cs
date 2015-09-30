namespace EA.Iws.Core.Shared
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Prsd.Core.Validation;

    public class ContactData : IValidatableObject
    {
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Telephone number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"[\d]+[\d\s]+[\d]+", ErrorMessage = "The telephone number is invalid")]
        public string Telephone { get; set; }

        [Display(Name = "Fax number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"[\d]+[\d\s]+[\d]+", ErrorMessage = "The fax number is invalid")]
        public string Fax { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telephone prefix")]
        [StringLength(3)]
        [RegularExpression("\\d+", ErrorMessage = "The telephone number is invalid")]
        public string TelephonePrefix { get; set; }

        [Display(Name = "Fax prefix")]
        [StringLength(3)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("\\d+", ErrorMessage = "The fax number is invalid")]
        public string FaxPrefix { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ((!string.IsNullOrEmpty(Fax) || !string.IsNullOrEmpty(FaxPrefix)) && (string.IsNullOrEmpty(Fax) || string.IsNullOrEmpty(FaxPrefix)))
            {
                yield return new ValidationResult("Please provide a valid fax number", new[] { "FaxPrefix" });
            }
        }
    }
}