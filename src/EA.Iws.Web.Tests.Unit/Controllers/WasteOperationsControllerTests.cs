namespace EA.Iws.Web.Tests.Unit.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.WasteOperations;
    using FakeItEasy;
    using Xunit;

    public class WasteOperationsControllerTests
    {
        private readonly IIwsClient client;
        private readonly Guid notificationId = Guid.NewGuid();
        private readonly WasteOperationsController wasteOperationsController;

        public WasteOperationsControllerTests()
        {
            client = A.Fake<IIwsClient>();
            wasteOperationsController = new WasteOperationsController(() => client);
        }

        [Fact]
        public async Task TechnologyEmployed_GetByNotificationId_RedirectsTo_View()
        {
            var result = await wasteOperationsController.TechnologyEmployed(notificationId) as ViewResult;
            Assert.NotNull(result);
            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Valid_TechnologyEmployed_RedirectsTo_ReasonForExport()
        {
            var model = new TechnologyEmployedViewModel()
            {
                NotificationId = notificationId,
                Details = "any valid value",
                FurtherDetails = "any value",
                AnnexProvided = false
            };

            var result = await wasteOperationsController.TechnologyEmployed(model) as RedirectToRouteResult;
            Assert.NotNull(result);
            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("ReasonForExport", result.RouteValues["controller"]);
            Assert.Equal(notificationId, result.RouteValues["id"]);
        }

        [Fact]
        public async Task Invalid_TechnologyEmployed_Returns_Same_View()
        {
            var model = new TechnologyEmployedViewModel();
            wasteOperationsController.ModelState.AddModelError("Test", "Error");

            var result = await wasteOperationsController.TechnologyEmployed(model) as ViewResult;
            Assert.Equal(string.Empty, result.ViewName);
        }
    }
}
