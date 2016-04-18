namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.WasteGenerationProcess;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Xunit;

    public class WasteGenerationProcessControllerTests
    {
        private readonly WasteGenerationProcessController wasteGenerationProcessController;

        public WasteGenerationProcessControllerTests()
        {
            wasteGenerationProcessController = new WasteGenerationProcessController(A.Fake<IMediator>());
        }

        [Fact]
        public async Task Post_BackToOverviewTrue_RedirectsToOverview()
        {
            var model = new WasteGenerationProcessViewModel();
            var result = await wasteGenerationProcessController.Index(model, true) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "Home");
        }

        [Fact]
        public async Task Post_BackToOverviewFalse_RedirectsToPhysicalCharacteristics()
        {
            var model = new WasteGenerationProcessViewModel();
            var result = await wasteGenerationProcessController.Index(model, false) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "PhysicalCharacteristics");
        }
    }
}
