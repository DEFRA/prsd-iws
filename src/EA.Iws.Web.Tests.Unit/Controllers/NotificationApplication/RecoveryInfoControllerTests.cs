namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
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
        private const double fullyRecoverablePercentage = 100;
        private const double partiallyRecoverablePercentage = 12;

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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(null)]
        public async Task RecoveryPercentage_Post_WhenNotProvidedByImporter_IgnoresBackToOverview(bool? backToOverview)
        {
            var model = new RecoveryPercentageViewModel() 
            {
                NotificationId = notificationId,
                IsProvidedByImporter = false,
                PercentageRecoverable = 12.34M
            };

            var result = await recoveryInfoController.RecoveryPercentage(model, backToOverview) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "MethodOfDisposal", "RecoveryInfo");
        }

        [Theory]
        [InlineData(true, fullyRecoverablePercentage)]
        [InlineData(false, fullyRecoverablePercentage)]
        [InlineData(null, fullyRecoverablePercentage)]
        [InlineData(true, partiallyRecoverablePercentage)]
        [InlineData(false, partiallyRecoverablePercentage)]
        [InlineData(null, partiallyRecoverablePercentage)]
        public async Task RecoveryPercentage_Post_WhenNotProvidedByImporter_BackToOverview_MaintainsRouteValue(bool? backToOverview, double recoverablePercentage)
        {
            var model = new RecoveryPercentageViewModel()
            {
                NotificationId = notificationId,
                IsProvidedByImporter = false,
                PercentageRecoverable = (decimal)recoverablePercentage
            };

            var result = await recoveryInfoController.RecoveryPercentage(model, backToOverview) as RedirectToRouteResult;

            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.Equal<bool?>(backToOverview.GetValueOrDefault(),
                ((bool?)result.RouteValues[backToOverviewKey]).GetValueOrDefault());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(null)]
        public async Task MethodOfDisposal_Post_BackToOverview_MaintainsRouteValue(bool? backToOverview)
        {
            var model = new MethodOfDisposalViewModel();
            var result = await recoveryInfoController.MethodOfDisposal(model, backToOverview) as RedirectToRouteResult;
            
            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.Equal<bool?>(backToOverview.GetValueOrDefault(),
                ((bool?)result.RouteValues[backToOverviewKey]).GetValueOrDefault());
        }

        [Fact]
        public async Task RecoveryValues_Post_BackToOverviewTrue_RedirectsToOverview()
        {
            var model = new RecoveryInfoValuesViewModel();
            var result = await recoveryInfoController.RecoveryValues(model, true) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "Home");
        }
        
        [Fact]
        public async Task RecoveryValues_Post_BackToOverviewFalse_RedirectsToOverview()
        {
            var model = new RecoveryInfoValuesViewModel();
            var result = await recoveryInfoController.RecoveryValues(model, false) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "Home");
        }
    }
}
