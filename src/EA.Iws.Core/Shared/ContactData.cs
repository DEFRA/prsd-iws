namespace EA.Iws.Core.Shared
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ContactData : IValidatableObject
    {
        [Required(ErrorMessageResourceType = typeof(ContactDataResources), ErrorMessageResourceName = "FullNameRequired")]
        [Display(Name = "FullName", ResourceType = typeof(ContactDataResources))]
        public string FullName { get; set; }

        [Required(ErrorMessageResourceType = typeof(ContactDataResources), ErrorMessageResourceName = "TelephoneNumberRequired")]
        [Display(Name = "TelephoneNumber", ResourceType = typeof(ContactDataResources))]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"[\d]+[\d\s]+[\d]+", ErrorMessageResourceType = typeof(ContactDataResources), ErrorMessageResourceName = "TelephoneInvalid")]
        public string Telephone { get; set; }

        [Display(Name = "FaxNumber", ResourceType = typeof(ContactDataResources))]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"[\d]+[\d\s]+[\d]+", ErrorMessageResourceType = typeof(ContactDataResources), ErrorMessageResourceName = "FaxInvalid")]
        public string Fax { get; set; }

        [Required(ErrorMessageResourceType = typeof(ContactDataResources), ErrorMessageResourceName = "TelephonePrefixRequired")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "TelephonePrefix", ResourceType = typeof(ContactDataResources))]
        [StringLength(3)]
        [RegularExpression("\\d+", ErrorMessageResourceType = typeof(ContactDataResources), ErrorMessageResourceName = "TelephoneInvalid")]
        public string TelephonePrefix { get; set; }

        [Display(Name = "FaxPrefix", ResourceType = typeof(ContactDataResources))]
        [StringLength(3)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("\\d+", ErrorMessageResourceType = typeof(ContactDataResources), ErrorMessageResourceName = "FaxInvalid")]
        public string FaxPrefix { get; set; }

        [Required(ErrorMessageResourceType = typeof(ContactDataResources), ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(ContactDataResources))]
        public string Email { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ((!string.IsNullOrWhiteSpace(Fax) || !string.IsNullOrWhiteSpace(FaxPrefix))
                && (string.IsNullOrWhiteSpace(Fax) || string.IsNullOrWhiteSpace(FaxPrefix)))
            {
                yield return new ValidationResult(ContactDataResources.FaxRequired, new[] { "FaxPrefix" });
            }
        }
    }
}