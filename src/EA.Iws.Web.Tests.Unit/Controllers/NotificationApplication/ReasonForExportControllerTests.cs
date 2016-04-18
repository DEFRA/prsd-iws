namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.NotificationApplication;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Xunit;

    public class ReasonForExportControllerTests
    {
        private readonly ReasonForExportController reasonForExportController;

        public ReasonForExportControllerTests()
        {
            reasonForExportController = new ReasonForExportController(A.Fake<IMediator>());
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
