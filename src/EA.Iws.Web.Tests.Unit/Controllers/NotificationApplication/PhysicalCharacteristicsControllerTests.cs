namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.PhysicalCharacteristics;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Web.ViewModels.Shared;
    using Xunit;

    public class PhysicalCharacteristicsControllerTests
    {
        private readonly PhysicalCharacteristicsController physicalCharacteristicsController;

        public PhysicalCharacteristicsControllerTests()
        {
            physicalCharacteristicsController = new PhysicalCharacteristicsController(A.Fake<IMediator>());
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
