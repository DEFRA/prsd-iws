namespace EA.Iws.Web.Tests.Unit.Tests.Controllers
{
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
            var registerViewModel = GetValidRegisterViewModel();

            registerViewModel.Name = string.Empty;

            var accountController = GetMockAccountController(registerViewModel);

            var result = await accountController.SubmitApplicantRegistration(registerViewModel) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task ApplicantRegister_SurnameNotProvided_ValidationError()
        {
            var registerViewModel = GetValidRegisterViewModel();

            registerViewModel.Surname = string.Empty;

            var accountController = GetMockAccountController(registerViewModel);

            var result = await accountController.SubmitApplicantRegistration(registerViewModel) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task ApplicantRegister_OrganistionNotProvided_ValidationError()
        {
            var registerViewModel = GetValidRegisterViewModel();

            registerViewModel.OrganisationName = string.Empty;

            var accountController = GetMockAccountController(registerViewModel);

            var result = await accountController.SubmitApplicantRegistration(registerViewModel) as ViewResult;

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

            var result = await accountController.SubmitApplicantRegistration(registerViewModel) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task ApplicantRegister_TermsAndConditionsNotChecked_ValidationError()
        {
            var registerViewModel = GetValidRegisterViewModel();

            registerViewModel.TermsAndConditions = false;

            var accountController = GetMockAccountController(registerViewModel);

            var result = await accountController.SubmitApplicantRegistration(registerViewModel) as ViewResult;

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
                Surname = ValidSurname,
                TermsAndConditions = true
            };

            return validRegisterViewModel;
        }
    }
}
