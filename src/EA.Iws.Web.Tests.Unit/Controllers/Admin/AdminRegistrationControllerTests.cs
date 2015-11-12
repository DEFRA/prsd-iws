namespace EA.Iws.Web.Tests.Unit.Controllers.Admin
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Areas.Admin.ViewModels;
    using Areas.Admin.ViewModels.Registration;
    using Core.Notification;
    using FakeItEasy;
    using Prsd.Core.Web.OAuth;
    using Services;
    using Xunit;

    public class AdminRegistrationControllerTests
    {
        public AdminRegistrationControllerTests()
        {
            var dependencyResolver = A.Fake<IDependencyResolver>();
            A.CallTo(() => dependencyResolver.GetService(typeof(AppConfiguration))).Returns(
                new AppConfiguration()
                {
                    Environment = "LIVE"
                });
            DependencyResolver.SetResolver(dependencyResolver);
        }

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
            var oauth = A.Fake<IOAuthClient>();
            var iwsClient = A.Fake<IIwsClient>();
            var registrationController = new Areas.Admin.Controllers.RegistrationController(() => oauth, iwsClient, null);
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
            const CompetentAuthority ValidCA = CompetentAuthority.England;
            const string ValidJobTitle = "Title";
            Guid validLocalAreaId = new Guid("BA16D091-6C2A-410A-84CE-8689D2CE2EFF");

            var validRegisterViewModel = new AdminRegistrationViewModel
            {
                Email = ValidEmail,
                Password = ValidPassword,
                ConfirmPassword = ValidPassword,
                Name = ValidName,
                Surname = ValidSurname,
                CompetentAuthority = ValidCA,
                JobTitle = ValidJobTitle,
                LocalAreaId = validLocalAreaId
            };

            return validRegisterViewModel;
        }
    }
}
