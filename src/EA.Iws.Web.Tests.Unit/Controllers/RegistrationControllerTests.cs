namespace EA.Iws.Web.Tests.Unit.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Prsd.Core.Web.OAuth;
    using Web.Controllers;
    using Web.ViewModels.Registration;
    using Xunit;

    public class RegistrationControllerTests
    {
        private const string ValidEmail = "test@test.com";
        private const string ValidPassword = "P@ssword1";
        private const string ValidOrganisationName = "OrgName";
        private const string ValidPhoneNumber = "01243234567";
        private const string ValidName = "ValidName";
        private const string ValidSurname = "ValidSurname";
        private static readonly Guid UserIdGuid = new Guid();

        [Fact]
        public async Task ApplicantRegister_NameNotProvided_ValidationError()
        {
            var registerViewModel = GetValidRegisterViewModel();

            registerViewModel.Name = string.Empty;

            var accountController = GetMockAccountController(registerViewModel);

            var result = await accountController.ApplicantRegistration(registerViewModel) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task ApplicantRegister_SurnameNotProvided_ValidationError()
        {
            var registerViewModel = GetValidRegisterViewModel();

            registerViewModel.Surname = string.Empty;

            var accountController = GetMockAccountController(registerViewModel);

            var result = await accountController.ApplicantRegistration(registerViewModel) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task ApplicantRegister_OrganistionNotProvided_ValidationError()
        {
            var registerViewModel = GetValidRegisterViewModel();

            registerViewModel.OrganisationName = string.Empty;

            var accountController = GetMockAccountController(registerViewModel);

            var result = await accountController.ApplicantRegistration(registerViewModel) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData("test")]
        [InlineData("test@")]
        [InlineData("test.com")]
        public async Task ApplicantRegister_EmailInvalidOrNotProvided_ValidationError(string expected)
        {
            var registerViewModel = GetValidRegisterViewModel();

            registerViewModel.Email = expected;

            var accountController = GetMockAccountController(registerViewModel);

            var result = await accountController.ApplicantRegistration(registerViewModel) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task ApplicantRegister_TermsAndConditionsNotChecked_ValidationError()
        {
            var registerViewModel = GetValidRegisterViewModel();

            registerViewModel.TermsAndConditions = false;

            var accountController = GetMockAccountController(registerViewModel);

            var result = await accountController.ApplicantRegistration(registerViewModel) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void ApplicantRegister_PasswordsDoNotMatch_ValidationError()
        {
            var registerViewModel = GetValidRegisterViewModel();

            registerViewModel.TermsAndConditions = false;

            var validationContext = new ValidationContext(registerViewModel, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(registerViewModel, validationContext, validationResults, true);

            Assert.True(validationResults.Any(vr => vr.ErrorMessage.Equals("Please confirm that you have read the terms and conditions")));
        }

        [Fact]
        public async Task EditApplicantDetails_FirstName_Required_ValidationError()
        {
            var editApplicantDetails = GetEditApplicantDetailsViewModel();
            editApplicantDetails.FirstName = string.Empty;

            var registrationController = GetMockAccountController(editApplicantDetails);

            var result = await registrationController.EditApplicantDetails(editApplicantDetails) as ViewResult;
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task EditApplicantDetails_Surname_Required_ValidationError()
        {
            var editApplicantDetails = GetEditApplicantDetailsViewModel();
            editApplicantDetails.Surname = string.Empty;

            var registrationController = GetMockAccountController(editApplicantDetails);

            var result = await registrationController.EditApplicantDetails(editApplicantDetails) as ViewResult;
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        private static RegistrationController GetMockAccountController(object viewModel)
        {
            var registrationController = new RegistrationController(() => new OAuthClient("test", "test", "test"), () => new IwsClient("test"), null, null);
            // Mimic the behaviour of the model binder which is responsible for Validating the Model
            var validationContext = new ValidationContext(viewModel, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(viewModel, validationContext, validationResults, true);
            foreach (var validationResult in validationResults)
            {
                registrationController.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }

            return registrationController;
        }

        private static ApplicantRegistrationViewModel GetValidRegisterViewModel()
        {
            var validRegisterViewModel = new ApplicantRegistrationViewModel
            {
                Email = ValidEmail,
                Password = ValidPassword,
                ConfirmPassword = ValidPassword,
                OrganisationName = ValidOrganisationName,
                PhoneNumber = ValidPhoneNumber,
                Name = ValidName,
                Surname = ValidSurname,
                TermsAndConditions = true
            };

            return validRegisterViewModel;
        }

        private static EditApplicantDetailsViewModel GetEditApplicantDetailsViewModel()
        {
            return new EditApplicantDetailsViewModel()
            {
                Id = UserIdGuid,
                FirstName = ValidName,
                Surname = ValidSurname,
                Email = ValidEmail,
                ExistingEmail = ValidEmail,
                Password = ValidPassword
            };
        }
    }
}
