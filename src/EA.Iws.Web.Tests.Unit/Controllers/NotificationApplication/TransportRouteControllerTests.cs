namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using Areas.NotificationApplication.Controllers;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using System;
    using System.Web.Mvc;
    using Xunit;

    public class TransportRouteControllerTests
    {
        private readonly Guid notificationId = new Guid();
        private readonly TransportRouteController transportRouteController;

        public TransportRouteControllerTests()
        {
            transportRouteController = new TransportRouteController(A.Fake<IMediator>());
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
