namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.MeansOfTransport;
    using Core.Notification.Audit;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Web.Infrastructure;
    using Xunit;

    public class MeansOfTransportControllerTests
    {
        private readonly Guid notificationId = new Guid("09237AF4-F46B-4191-AAB7-6404D0A1A751");
        private readonly MeansOfTransportController meansOfTransportController;
        private readonly IMediator mediator;
        private readonly IAuditService auditService;

        public MeansOfTransportControllerTests()
        {
            this.mediator = A.Fake<IMediator>();
            this.auditService = A.Fake<IAuditService>();
            meansOfTransportController = new MeansOfTransportController(A.Fake<IMediator>(), this.auditService);
            A.CallTo(() => auditService.AddAuditEntry(this.mediator, notificationId, "user", NotificationAuditType.Create, "screen"));
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
