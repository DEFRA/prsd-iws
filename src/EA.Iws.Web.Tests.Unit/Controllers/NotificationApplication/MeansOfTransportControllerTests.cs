namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.MeansOfTransport;
    using FakeItEasy;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Xunit;

    public class MeansOfTransportControllerTests
    {
        private readonly Guid notificationId = new Guid("09237AF4-F46B-4191-AAB7-6404D0A1A751");
        private readonly MeansOfTransportController meansOfTransportController;

        public MeansOfTransportControllerTests()
        {
            meansOfTransportController = new MeansOfTransportController(A.Fake<IMediator>());
        }

        private MeansOfTransportViewModel CreateValidMeansOfTransportViewModel()
        {
            return new MeansOfTransportViewModel()
            {
                SelectedMeans = "r-t-s-a-w",
            };
        }

        [Fact]
        public async Task Post_BackToOverviewTrue_RedirectsToOverview()
        {
            var model = CreateValidMeansOfTransportViewModel();

            var result = await meansOfTransportController.Index(notificationId, model, true) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "Index", "Home");
        }

        [Fact]
        public async Task Post_BackToOverviewFalse_RedirectsToPackagingTypes()
        {
            var model = CreateValidMeansOfTransportViewModel();

            var result = await meansOfTransportController.Index(notificationId, model, false) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "Index", "PackagingTypes");
        }
    }
}
