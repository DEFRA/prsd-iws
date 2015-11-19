namespace EA.Iws.Web.ViewModels.Registration
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Api.Client.Entities;
    using Views.Registration;

    public class EditApplicantDetailsViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(EditApplicantDetailsResources), ErrorMessageResourceName = "FirstNameRequired")]
        [Display(Name = "FirstName", ResourceType = typeof(EditApplicantDetailsResources))]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(EditApplicantDetailsResources), ErrorMessageResourceName = "LastNameRequired")]
        [StringLength(50)]
        [DataType(DataType.Text)]
        [Display(Name = "LastName", ResourceType = typeof(EditApplicantDetailsResources))]
        public string Surname { get; set; }

        [Required(ErrorMessageResourceType = typeof(EditApplicantDetailsResources), ErrorMessageResourceName = "TelephoneNumberRequired")]
        [DataType(DataType.PhoneNumber, ErrorMessageResourceType = typeof(EditApplicantDetailsResources), ErrorMessageResourceName = "TelephoneFormatValidation")]
        [Display(Name = "TelephoneNumber", ResourceType = typeof(EditApplicantDetailsResources))]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessageResourceType = typeof(EditApplicantDetailsResources), ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress(ErrorMessageResourceType = typeof(EditApplicantDetailsResources), ErrorMessageResourceName = "EmailFormatValidation", ErrorMessage = null)]
        [Display(Name = "Email", ResourceType = typeof(EditApplicantDetailsResources))]
        public string Email { get; set; }

        public string ExistingEmail { get; set; }

        public bool IsEmailChanged
        {
            get { return !Email.Trim().Equals(ExistingEmail.Trim()); }
        }

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
                Email = Email
            };
        }
    }
}