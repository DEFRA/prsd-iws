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
        private const string MethodOfDisposal = "some disposal method description";

        [Fact]
        public async Task IsNotProvidedByImporter_IsHundredPercentRecoverableIsNull_ValidationError()
        {
            var viewModel = new RecoveryPercentageViewModel();
            viewModel.IsProvidedByImporter = false;
            viewModel.IsHundredPercentRecoverable = null;

            var controller = GetMockRecoveryInfoController(viewModel);
            var result = await controller.RecoveryPercentage(viewModel) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task IsProvidedByImporter_IsHundredPercentRecoverableIsFalse_PercentageRecoverableIsNull_ValidationError()
        {
            var viewModel = new RecoveryPercentageViewModel();
            viewModel.IsHundredPercentRecoverable = false;
            viewModel.PercentageRecoverable = null;

            var controller = GetMockRecoveryInfoController(viewModel);
            var result = await controller.RecoveryPercentage(viewModel) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task IsProvidedByImporter_IsHundredPercentRecoverableIsFalse_MethodOfDisposalIsNull_ValidationError()
        {
            var viewModel = new RecoveryPercentageViewModel();
            viewModel.IsHundredPercentRecoverable = false;
            viewModel.MethodOfDisposal = null;

            var controller = GetMockRecoveryInfoController(viewModel);
            var result = await controller.RecoveryPercentage(viewModel) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task IsProvidedByImporter_IsHundredPercentRecoverableIsFalse_MethodOfDisposalIsEmpty_ValidationError()
        {
            var viewModel = new RecoveryPercentageViewModel();
            viewModel.IsHundredPercentRecoverable = false;
            viewModel.MethodOfDisposal = string.Empty;

            var controller = GetMockRecoveryInfoController(viewModel);
            var result = await controller.RecoveryPercentage(viewModel) as ViewResult;

            Assert.False(result.ViewData.ModelState.IsValid);
        }

        private static RecoveryInfoController GetMockRecoveryInfoController(object viewModel)
        {
            var client = A.Fake<IIwsClient>();

            var controller = new RecoveryInfoController(() => client);
            // Mimic the behaviour of the model binder which is responsible for Validating the Model
            var validationContext = new ValidationContext(viewModel, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(viewModel, validationContext, validationResults, true);
            foreach (var validationResult in validationResults)
            {
                controller.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }

            return controller;
        }
    }
}
