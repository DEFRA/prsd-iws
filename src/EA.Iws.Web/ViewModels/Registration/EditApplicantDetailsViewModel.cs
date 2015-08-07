namespace EA.Iws.Web.ViewModels.Registration
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Api.Client.Entities;
    using Prsd.Core.Validation;

    public class EditApplicantDetailsViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "First name")]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Text)]
        [Display(Name = "Last name")]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telephone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        public string ExistingEmail { get; set; }

        public bool IsEmailChanged
        {
            get { return !Email.Trim().Equals(ExistingEmail.Trim()); }
        }

        [RequiredIf("IsEmailChanged", true, "Please enter your password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public EditApplicantDetailsViewModel()
        {
        }

        public EditApplicantDetailsViewModel(EditApplicantRegistrationData data)
        {
            Id = data.Id;
            FirstName = data.FirstName;
            Surname = data.Surname;
            PhoneNumber = data.Phone;
            Email = data.Email;
            ExistingEmail = data.Email;
        }

        public EditApplicantRegistrationData ToRequest()
        {
            return new EditApplicantRegistrationData
            {
                Id = Id,
                FirstName = FirstName,
                Surname = Surname,
                Phone = PhoneNumber,
                Email = Email,
                Password = Password
            };
        }
    }
}