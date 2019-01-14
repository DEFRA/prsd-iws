namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.SpecialHandling;
    using Core.Notification.Audit;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Web.Infrastructure;
    using Xunit;

    public class SpecialHandlingControllerTests
    {
        private readonly SpecialHandlingController specialHandlingController;
        private readonly Guid notificationId = new Guid("4AB23CDF-9B24-4598-A302-A69EBB5F2152");
        private readonly IMediator mediator;
        private readonly IAuditService auditService;

        public SpecialHandlingControllerTests()
        {
            this.mediator = A.Fake<IMediator>();
            this.auditService = A.Fake<IAuditService>();
            specialHandlingController = new SpecialHandlingController(A.Fake<IMediator>(), this.auditService);
            A.CallTo(() => auditService.AddAuditEntry(this.mediator, notificationId, "user", NotificationAuditType.Create, "screen"));
        }

        [Fact]
        public async Task Post_BackToOverviewTrue_RedirectsToOverview()
        {
            var model = new SpecialHandlingViewModel();
            var result = await specialHandlingController.Index(model, true) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "Home");
        }

        [Fact]
        public async Task Post_BackToOverviewFalse_RedirectsToStateOfExport()
        {
            var model = new SpecialHandlingViewModel();
            var result = await specialHandlingController.Index(model, false) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "StateOfExport");
        }
    }
}
