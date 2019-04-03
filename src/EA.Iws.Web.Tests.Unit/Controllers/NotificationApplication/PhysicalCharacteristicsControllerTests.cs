namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.PhysicalCharacteristics;
    using Core.Notification.Audit;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Web.Infrastructure;
    using Web.ViewModels.Shared;
    using Xunit;

    public class PhysicalCharacteristicsControllerTests
    {
        private readonly PhysicalCharacteristicsController physicalCharacteristicsController;
        private readonly IAuditService auditService;
        private readonly IMediator mediator;
        private readonly Guid notificationId = new Guid("DD1F019D-BD85-4A6F-89AB-328A7BD53CEA");

        public PhysicalCharacteristicsControllerTests()
        {
            this.mediator = A.Fake<IMediator>();
            this.auditService = A.Fake<IAuditService>();
            physicalCharacteristicsController = new PhysicalCharacteristicsController(this.mediator, this.auditService);
            A.CallTo(() => auditService.AddAuditEntry(this.mediator, notificationId, "user", NotificationAuditType.Added, NotificationAuditScreenType.PhysicalCharacteristics));
        }

        [Fact]
        public async Task Post_BackToOverviewTrue_RedirectsToOverview()
        {
            var physicalCharacteristics = new CheckBoxCollectionViewModel();
            physicalCharacteristics.PossibleValues = new[] { new SelectListItem() };
            var model = new PhysicalCharacteristicsViewModel()
            {
                PhysicalCharacteristics = physicalCharacteristics
            };
            var result = await physicalCharacteristicsController.Index(model, true) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "Home");
        }

        [Fact]
        public async Task Post_BackToOverviewFalse_RedirectsToWasteCodes()
        {
            var physicalCharacteristics = new CheckBoxCollectionViewModel();
            physicalCharacteristics.PossibleValues = new[] { new SelectListItem() };
            var model = new PhysicalCharacteristicsViewModel()
            {
                PhysicalCharacteristics = physicalCharacteristics
            };
            var result = await physicalCharacteristicsController.Index(model, false) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "BaselOecdCode");
        }
    }
}
