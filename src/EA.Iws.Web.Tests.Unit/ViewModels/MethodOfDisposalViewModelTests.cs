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

    public class MethodOfDisposalViewModelTests
    {
        [Fact]
        public async Task PercentRecoverableIsLessThanZero_ValidationError()
        {
            var viewModel = new MethodOfDisposalViewModel();
            viewModel.PercentageRecoverable = -10M;

            var controller = GetMockRecoveryInfoController(viewModel);
            var result = await controller.MethodOfDisposal(viewModel) as ViewResult;

            Assert.False(result != null && result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task PercentRecoverableIsGreaterThanHundred_ValidationError()
        {
            var viewModel = new MethodOfDisposalViewModel();
            viewModel.PercentageRecoverable = 200M;

            var controller = GetMockRecoveryInfoController(viewModel);
            var result = await controller.MethodOfDisposal(viewModel) as ViewResult;

            Assert.False(result != null && result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public async Task PercentRecoverableIsValid_MethodOfDisposalIsNull_ValidationError()
        {
            var viewModel = new MethodOfDisposalViewModel();
            viewModel.PercentageRecoverable = 200M;
            viewModel.MethodOfDisposal = null;

            var controller = GetMockRecoveryInfoController(viewModel);
            var result = await controller.MethodOfDisposal(viewModel) as ViewResult;

            Assert.False(result != null && result.ViewData.ModelState.IsValid);
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
