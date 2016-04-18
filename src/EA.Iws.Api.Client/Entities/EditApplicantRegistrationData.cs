namespace EA.Iws.Api.Client.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class EditApplicantRegistrationData
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string Surname { get; set; }

        [RegularExpression("^[0-9+\\(\\)#\\.\\s\\/ext-]+$", ErrorMessage = "The entered phone number is invalid")]
        [DataType(DataType.Text)]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}