namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using EA.Iws.Api.Client;
    using EA.Iws.Web.Areas.NotificationApplication.Controllers;
    using FakeItEasy;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Xunit;

    public class TransportRouteControllerTests
    {
        private readonly IIwsClient client;
        private readonly Guid notificationId = new Guid();
        private readonly TransportRouteController transportRouteController;

        public TransportRouteControllerTests()
        {
            client = A.Fake<IIwsClient>();
            transportRouteController = new TransportRouteController(() => client);
        }

        [Fact]
        public void Post_BackToOverviewTrue_RedirectsToOverview()
        {
            var model = new FormCollection();
            var result = transportRouteController.Summary(notificationId, model, true) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "Home");
        }

        [Fact]
        public void Post_BackToOverviewFalse_RedirectsToOverview()
        {
            var model = new FormCollection();
            var result = transportRouteController.Summary(notificationId, model, false) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "CustomsOffice");
        }
    }
}
