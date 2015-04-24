namespace EA.Iws.Web.Tests.Unit.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.Registration;
    using Web.Controllers;
    using Xunit;

    public class RegistrationControllerTests
    {
        [Fact]
        public async Task ApplicantRegister_NameNotProvided_ValidationError()
        {
            var validRegisterViewModel = GetValidRegisterViewModel();

            validRegisterViewModel.Name = string.Empty;

            var accountController = GetMockAccountController(validRegisterViewModel);

            var result = await accountController.SubmitApplicantRegistration(validRegisterViewModel) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task ApplicantRegister_SurnameNotProvided_ValidationError()
        {
            var validRegisterViewModel = GetValidRegisterViewModel();

            validRegisterViewModel.Surname = string.Empty;

            var accountController = GetMockAccountController(validRegisterViewModel);

            var result = await accountController.SubmitApplicantRegistration(validRegisterViewModel) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task ApplicantRegister_OrganistionNotProvided_ValidationError()
        {
            var validRegisterViewModel = GetValidRegisterViewModel();

            validRegisterViewModel.OrganisationName = string.Empty;

            var accountController = GetMockAccountController(validRegisterViewModel);

            var result = await accountController.SubmitApplicantRegistration(validRegisterViewModel) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData("test")]
        [InlineData("test@")]
        [InlineData("test.com")]
        public async Task ApplicantRegister_EmailInvalidOrNotProvided_ValidationError()
        {
            var validRegisterViewModel = GetValidRegisterViewModel();

            validRegisterViewModel.Email = string.Empty;

            var accountController = GetMockAccountController(validRegisterViewModel);

            var result = await accountController.SubmitApplicantRegistration(validRegisterViewModel) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData("01233xcx2332")]
        [InlineData("text")]
        public async Task ApplicantRegister_PhoneInvalidOrNotProvided_ValidationError()
        {
            var validRegisterViewModel = GetValidRegisterViewModel();

            validRegisterViewModel.PhoneNumber = string.Empty;

            var accountController = GetMockAccountController(validRegisterViewModel);

            var result = await accountController.SubmitApplicantRegistration(validRegisterViewModel) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void ApplicantRegister_PasswordsDoNotMatch_ValidationError()
        {
            const String nonMatchingPassword = "NonMatchingPassword";

            var registerViewModel = GetValidRegisterViewModel();

            registerViewModel.ConfirmPassword = nonMatchingPassword;

            var validationContext = new ValidationContext(registerViewModel, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(registerViewModel, validationContext, validationResults, true);

            Assert.Equal("The password and confirmation password do not match.", validationResults[0].ErrorMessage);
        }

        private static RegistrationController GetMockAccountController(object viewModel)
        {
            var accountController = new RegistrationController();
            // Mimic the behaviour of the model binder which is responsible for Validating the Model
            var validationContext = new ValidationContext(viewModel, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(viewModel, validationContext, validationResults, true);
            foreach (var validationResult in validationResults)
            {
                accountController.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }

            return accountController;
        }

        private static ApplicantRegistrationViewModel GetValidRegisterViewModel()
        {
            const string ValidEmail = "test@test.com";
            const string ValidPassword = "P@ssword1";
            const string ValidOrganisationName = "OrgName";
            const string ValidPhoneNumber = "01243234567";
            const string ValidName = "ValidName";
            const string ValidSurname = "ValidSurname";

            var validRegisterViewModel = new ApplicantRegistrationViewModel
            {
                Email = ValidEmail,
                Password = ValidPassword,
                ConfirmPassword = ValidPassword,
                OrganisationName = ValidOrganisationName,
                PhoneNumber = ValidPhoneNumber,
                Name = ValidName,
                Surname = ValidSurname
            };

            return validRegisterViewModel;
        }
    }
}
