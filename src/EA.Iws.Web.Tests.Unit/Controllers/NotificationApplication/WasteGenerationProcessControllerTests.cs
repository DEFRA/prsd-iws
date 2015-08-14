namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using EA.Iws.Api.Client;
    using EA.Iws.Web.Areas.NotificationApplication.Controllers;
    using EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteGenerationProcess;
    using FakeItEasy;
    using Xunit;

    public class WasteGenerationProcessControllerTests
    {
        private readonly IIwsClient client;
        private readonly WasteGenerationProcessController wasteGenerationProcessController;

        public WasteGenerationProcessControllerTests()
        {
            client = A.Fake<IIwsClient>();
            wasteGenerationProcessController = new WasteGenerationProcessController(() => client);
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
