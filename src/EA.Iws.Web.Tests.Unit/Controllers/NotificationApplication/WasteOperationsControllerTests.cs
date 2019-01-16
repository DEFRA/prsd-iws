namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.WasteOperations;
    using Core.Notification.Audit;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.TechnologyEmployed;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Web.Infrastructure;
    using Xunit;

    public class WasteOperationsControllerTests
    {
        private readonly Guid notificationId = Guid.NewGuid();
        private readonly WasteOperationsController wasteOperationsController;
        private readonly IMediator mediator;
        private readonly IAuditService auditService;

        public WasteOperationsControllerTests()
        {
            this.mediator = A.Fake<IMediator>();
            this.auditService = A.Fake<IAuditService>();
            wasteOperationsController = new WasteOperationsController(mediator, this.auditService);

            A.CallTo(() => mediator.SendAsync(A<SetTechnologyEmployed>.Ignored))
                .Returns(Guid.Empty);
            A.CallTo(() => auditService.AddAuditEntry(this.mediator, notificationId, "user", NotificationAuditType.Create, "screen"));
        }

        [Fact]
        public async Task TechnologyEmployed_GetByNotificationId_RedirectsTo_View()
        {
            var result = await wasteOperationsController.TechnologyEmployed(notificationId) as ViewResult;
            Assert.NotNull(result);
            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Invalid_TechnologyEmployed_Returns_Same_View()
        {
            var model = new TechnologyEmployedViewModel();
            wasteOperationsController.ModelState.AddModelError("Test", "Error");

            var result = await wasteOperationsController.TechnologyEmployed(model) as ViewResult;
            Assert.Equal(string.Empty, result.ViewName);
        }
    }
}