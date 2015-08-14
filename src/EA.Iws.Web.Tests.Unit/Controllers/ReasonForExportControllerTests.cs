namespace EA.Iws.Web.Tests.Unit.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.NotificationApplication;
    using FakeItEasy;
    using Xunit;

    public class ReasonForExportControllerTests
    {
        private readonly IIwsClient client;
        private readonly Guid notificationId = Guid.NewGuid();
        private readonly ReasonForExportController reasonForExportController;

        public ReasonForExportControllerTests()
        {
            client = A.Fake<IIwsClient>();
            reasonForExportController = new ReasonForExportController(() => client);
        }

        [Fact]
        public async Task GetByNotificationId_RedirectsTo_Index()
        {
            var result = await reasonForExportController.Index(notificationId) as ViewResult;
            Assert.NotNull(result);
            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Valid_ReasonForExport_RedirectsTo_Carrier()
        {
            var model = new ReasonForExportViewModel()
            {
                NotificationId = notificationId,
                ReasonForExport = "any valid value"
            };

            var result = await reasonForExportController.Index(model) as RedirectToRouteResult;
            Assert.NotNull(result);
            Assert.Equal("List", result.RouteValues["action"]);
            Assert.Equal("Carrier", result.RouteValues["controller"]);
            Assert.Equal(notificationId, result.RouteValues["id"]);
        }

        [Fact]
        public async Task Invalid_ReasonForExport_Returns_Same_View()
        {
            var model = new ReasonForExportViewModel();
            reasonForExportController.ModelState.AddModelError("Test", "Error");

            var result = await reasonForExportController.Index(model) as ViewResult;
            Assert.Equal(string.Empty, result.ViewName);
        }
    }
}
