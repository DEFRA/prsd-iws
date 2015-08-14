namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using EA.Iws.Api.Client;
    using EA.Iws.Web.Areas.NotificationApplication.Controllers;
    using EA.Iws.Web.Areas.NotificationApplication.ViewModels.MeansOfTransport;
    using FakeItEasy;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Xunit;

    public class MeansOfTransportControllerTests
    {
        private readonly IIwsClient client;
        private readonly Guid notificationId = new Guid("09237AF4-F46B-4191-AAB7-6404D0A1A751");
        private readonly MeansOfTransportController meansOfTransportController;

        public MeansOfTransportControllerTests()
        {
            client = A.Fake<IIwsClient>();
            meansOfTransportController = new MeansOfTransportController(() => client);
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
