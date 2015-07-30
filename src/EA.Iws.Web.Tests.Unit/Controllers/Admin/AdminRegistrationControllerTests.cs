namespace EA.Iws.Web.Tests.Unit.Controllers.Admin
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Areas.Admin.ViewModels;
    using Prsd.Core.Web.OAuth;
    using Xunit;

    public class AdminRegistrationControllerTests
    {
        [Fact]
        public async Task ApplicantRegister_NameNotProvided_ValidationError()
        {
            var registerViewModel = GetValidRegisterViewModel();

            registerViewModel.Name = string.Empty;

            var accountController = GetMockAccountController(registerViewModel);

            var result = await accountController.Register(registerViewModel) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task ApplicantRegister_SurnameNotProvided_ValidationError()
        {
            var registerViewModel = GetValidRegisterViewModel();

            registerViewModel.Surname = string.Empty;

            var accountController = GetMockAccountController(registerViewModel);

            var result = await accountController.Register(registerViewModel) as ViewResult;

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

            var result = await accountController.Register(registerViewModel) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        private static Areas.Admin.Controllers.RegistrationController GetMockAccountController(object viewModel)
        {
            var registrationController = new Areas.Admin.Controllers.RegistrationController(() => new OAuthClient("test", "test", "test"), () => new IwsClient("test"), null, null);
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

        private static AdminRegistrationViewModel GetValidRegisterViewModel()
        {
            const string ValidEmail = "test@test.com";
            const string ValidPassword = "P@ssword1";
            const string ValidName = "ValidName";
            const string ValidSurname = "ValidSurname";
            const string ValidCA = "EA";
            const string ValidJobTitle = "Title";
            const string ValidRegion = "North";

            var validRegisterViewModel = new AdminRegistrationViewModel
            {
                Email = ValidEmail,
                Password = ValidPassword,
                ConfirmPassword = ValidPassword,
                Name = ValidName,
                Surname = ValidSurname,
                CompetentAuthority = ValidCA,
                JobTitle = ValidJobTitle,
                LocalArea = ValidRegion
            };

            return validRegisterViewModel;
        }
    }
}
