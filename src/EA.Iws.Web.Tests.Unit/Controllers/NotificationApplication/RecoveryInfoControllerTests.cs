namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.RecoveryInfo;
    using Core.Shared;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.RecoveryInfo;
    using Xunit;

    public class RecoveryInfoControllerTests
    {
        private static readonly Guid AnyGuid = Guid.NewGuid();

        private readonly IMediator mediator;
        private readonly RecoveryInfoController controller;

        public RecoveryInfoControllerTests()
        {
            mediator = A.Fake<IMediator>();
            controller = new RecoveryInfoController(mediator);
        }

        [Fact]
        public async Task RedirectsToOverview_WhenProvidedByImporter()
        {
            var result = await controller.Index(AnyGuid, new RecoveryInfoViewModel(ProvidedBy.Importer));

            var routeResult = Assert.IsType<RedirectToRouteResult>(result);

            RouteAssert.RoutesTo(routeResult.RouteValues, "Index", "Home");
        }

        [Fact]
        public async Task RedirectsToCorrectScreen_WhenProvidedByNotifier()
        {
            var result = await controller.Index(AnyGuid, new RecoveryInfoViewModel(ProvidedBy.Notifier));

            var routeResult = Assert.IsType<RedirectToRouteResult>(result);

            RouteAssert.RoutesTo(routeResult.RouteValues, "RecoveryPercentage", "RecoveryInfo");
        }

        private void SetNotificationProvidedBy(Guid notificationId, ProvidedBy? providedBy)
        {
            A.CallTo(() =>
                mediator.SendAsync(
                    A<GetRecoveryInfoProvider>.That.Matches(r =>
                        r.NotificationId == notificationId)))
                .Returns(providedBy);
        }
    }
}
