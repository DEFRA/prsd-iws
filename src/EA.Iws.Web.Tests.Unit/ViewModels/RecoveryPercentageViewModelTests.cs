namespace EA.Iws.Web.Tests.Unit.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.RecoveryInfo;
    using FakeItEasy;
    using Xunit;

    public class RecoveryPercentageViewModelTests
    {
        [Fact]
        public async Task IsProvidedByImporterIsFalse_PercentageRecoverableIsNull_ValidationError()
        {
            var viewModel = new RecoveryPercentageViewModel();
            viewModel.IsProvidedByImporter = false;
            viewModel.PercentageRecoverable = null;

            var controller = GetMockRecoveryInfoController(viewModel);
            var result = await controller.RecoveryPercentage(viewModel) as ViewResult;

            Assert.False(result != null && result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task IsProvidedByImporterIsProvided_PercentageRecoverableIsProvided_ValidationError()
        {
            var viewModel = new RecoveryPercentageViewModel();
            viewModel.IsProvidedByImporter = true;
            viewModel.PercentageRecoverable = 12.34M;

            var controller = GetMockRecoveryInfoController(viewModel);
            var result = await controller.RecoveryPercentage(viewModel) as ViewResult;

            Assert.False(result != null && result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task IsProvidedByImporterIsFalse_PercentRecoverableIsLessThanZero_ValidationError()
        {
            var viewModel = new RecoveryPercentageViewModel();
            viewModel.IsProvidedByImporter = false;
            viewModel.PercentageRecoverable = -10M;

            var controller = GetMockRecoveryInfoController(viewModel);
            var result = await controller.RecoveryPercentage(viewModel) as ViewResult;

            Assert.False(result != null && result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task IsProvidedByImporterIsFalse_PercentRecoverableIsGreaterThanHundred_ValidationError()
        {
            var viewModel = new RecoveryPercentageViewModel();
            viewModel.IsProvidedByImporter = false;
            viewModel.PercentageRecoverable = 200M;

            var controller = GetMockRecoveryInfoController(viewModel);
            var result = await controller.RecoveryPercentage(viewModel) as ViewResult;

            Assert.False(result != null && result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task IsProvidedByImporterIsFalse_PercentRecoverableIsZero_IsValid()
        {
            var viewModel = new RecoveryPercentageViewModel();
            viewModel.IsProvidedByImporter = false;
            viewModel.PercentageRecoverable = 0M;

            var controller = GetMockRecoveryInfoController(viewModel);
            await controller.RecoveryPercentage(viewModel);

            Assert.True(controller.ModelState.IsValid);
        }

        [Fact]
        public async Task IsProvidedByImporterIsFalse_PercentRecoverableIsHundred_IsValid()
        {
            var viewModel = new RecoveryPercentageViewModel();
            viewModel.IsProvidedByImporter = false;
            viewModel.PercentageRecoverable = 100M;

            var controller = GetMockRecoveryInfoController(viewModel);
            await controller.RecoveryPercentage(viewModel);

            Assert.True(controller.ModelState.IsValid);
        }

        private static RecoveryInfoController GetMockRecoveryInfoController(object viewModel)
        {
            var client = A.Fake<IIwsClient>();

            var controller = new RecoveryInfoController(() => client);
            var validationContext = new ValidationContext(viewModel, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(viewModel, validationContext, validationResults, false);
            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }

            return controller;
        }
    }
}
