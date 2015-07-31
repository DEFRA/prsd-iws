namespace EA.Iws.Web.Tests.Unit.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.RecoveryInfo;
    using FakeItEasy;
    using Xunit;

    public class RecoveryInfoControllerTests
    {
        private readonly IIwsClient client;
        private readonly Guid notificationId = Guid.NewGuid();
        private readonly RecoveryInfoController recoveryInfoController;

        public RecoveryInfoControllerTests()
        {
            client = A.Fake<IIwsClient>();
            recoveryInfoController = new RecoveryInfoController(() => client);
        }

        [Fact]
        public async Task Invalid_RecoveryPercentageViewModel_Returns_View()
        {
            var model = new RecoveryPercentageViewModel();
            recoveryInfoController.ModelState.AddModelError("Test", "Error");

            var result = await recoveryInfoController.RecoveryPercentage(model) as ViewResult;
            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Invalid_MethodOfDisposalViewModel_Returns_View()
        {
            var model = new RecoveryPercentageViewModel();
            recoveryInfoController.ModelState.AddModelError("Test", "Error");

            var result = await recoveryInfoController.RecoveryPercentage(model) as ViewResult;
            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task RecoveryPercentage_IsProvidedByImporter_RedirectsTo_Index()
        {
            var model = new RecoveryPercentageViewModel
            {
                NotificationId = notificationId,
                IsProvidedByImporter = true
            };

            var result = await recoveryInfoController.RecoveryPercentage(model) as RedirectToRouteResult;
            Assert.Equal("Index", result.RouteValues["action"]);
        }

        [Fact]
        public async Task RecoveryPercentage_IsNotProvidedByImporter_ValidPercentageRecovery_RedirectsTo_MethodOfDisposal()
        {
            var model = new RecoveryPercentageViewModel
            {
                NotificationId = notificationId,
                IsProvidedByImporter = false,
                PercentageRecoverable = 12.34M
            };

            var result = await recoveryInfoController.RecoveryPercentage(model) as RedirectToRouteResult;
            Assert.Equal("MethodOfDisposal", result.RouteValues["action"]);
        }

        [Fact]
        public async Task RecoveryInfo_IsNotProvidedByImporter_ValidPercentageRecovery_ValidMethodOfDisposal_RedirectsTo_RecoveryValues()
        {
            var model = new MethodOfDisposalViewModel
            {
                NotificationId = notificationId,
                PercentageRecoverable = 12.34M,
                MethodOfDisposal = "any method of disposal description text"
            };

            var result = await recoveryInfoController.MethodOfDisposal(model) as RedirectToRouteResult;
            Assert.Equal("RecoveryValues", result.RouteValues["action"]);
        }
    }
}
