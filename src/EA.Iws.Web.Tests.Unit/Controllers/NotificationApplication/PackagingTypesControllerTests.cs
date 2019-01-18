namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.PackagingTypes;
    using Core.Notification.Audit;
    using Core.PackagingType;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Web.Infrastructure;
    using Web.ViewModels.Shared;
    using Xunit;

    public class PackagingTypesControllerTests
    {
        private readonly Guid notificationId = new Guid("DD1F019D-BD85-4A6F-89AB-328A7BD53CEA");
        private readonly PackagingTypesController packagingTypesController;
        private readonly IMediator mediator;
        private readonly IAuditService auditService;

        public PackagingTypesControllerTests()
        {
            this.mediator = A.Fake<IMediator>();
            this.auditService = A.Fake<IAuditService>();
            packagingTypesController = new PackagingTypesController(A.Fake<IMediator>(), this.auditService);
            A.CallTo(() => auditService.AddAuditEntry(this.mediator, notificationId, "user", NotificationAuditType.Create, NotificationAuditScreenType.PackagingTypes));
        }

        private PackagingTypesViewModel CreateValidPackagingTypesViewModel()
        {
            var packagingTypes = CheckBoxCollectionViewModel.CreateFromEnum<PackagingType>();
            var selectedValues = new[] { 1, 2 };
            packagingTypes.SetSelectedValues(selectedValues);
            return new PackagingTypesViewModel()
            {
                PackagingTypes = packagingTypes,
                NotificationId = notificationId
            };
        }

        [Fact]
        public async Task Post_BackToOverviewTrue_RedirectsToOverview()
        {
            var model = CreateValidPackagingTypesViewModel();
            var result = await packagingTypesController.Index(model, true) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "Home");
        }

        [Fact]
        public async Task Post_BackToOverviewFalse_RedirectsToSpecialHandling()
        {
            var model = CreateValidPackagingTypesViewModel();
            var result = await packagingTypesController.Index(model, false) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "SpecialHandling");
        }
    }
}
