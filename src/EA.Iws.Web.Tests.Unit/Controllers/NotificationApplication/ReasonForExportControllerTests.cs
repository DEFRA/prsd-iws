namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using EA.Iws.Api.Client;
    using EA.Iws.Web.Areas.NotificationApplication.Controllers;
    using EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication;
    using FakeItEasy;
    using Xunit;

    public class ReasonForExportControllerTests
    {
        private readonly IIwsClient client;
        private readonly ReasonForExportController reasonForExportController;

        public ReasonForExportControllerTests()
        {
            client = A.Fake<IIwsClient>();
            reasonForExportController = new ReasonForExportController(() => client);
        }

        [Fact]
        public async Task ReasonForExport_Post_BackToOverviewTrue_RedirectsToOverview()
        {
            var model = new ReasonForExportViewModel();

            var result = await reasonForExportController.Index(model, true) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "Index", "Home");
        }

        [Fact]
        public async Task ReasonForExport_Post_BackToOverviewFalse_RedirectsToCarrierList()
        {
            var model = new ReasonForExportViewModel();

            var result = await reasonForExportController.Index(model, false) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "List", "Carrier");
        }

        [Fact]
        public async Task ReasonForExport_Post_BackToOverviewNull_RedirectsToCarrierList()
        {
            var model = new ReasonForExportViewModel();

            var result = await reasonForExportController.Index(model, null) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "List", "Carrier");
        }
    }
}
