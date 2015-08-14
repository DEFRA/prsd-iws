namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using EA.Iws.Api.Client;
    using EA.Iws.Web.Areas.NotificationApplication.Controllers;
    using EA.Iws.Web.Areas.NotificationApplication.ViewModels.Shipment;
    using FakeItEasy;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Xunit;

    public class ShipmentControllerTests
    {
        private readonly IIwsClient client;
        private readonly ShipmentController shipmentController;

        public ShipmentControllerTests()
        {
            client = A.Fake<IIwsClient>();
            shipmentController = new ShipmentController(() => client);
        }

        [Fact]
        public async Task Post_BackToOverviewTrue_RedirectsToOverview()
        {
            var model = new ShipmentInfoViewModel();
            var result = await shipmentController.Index(model, true) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "Home");
        }

        [Fact]
        public async Task Post_BackToOverviewFalse_RedirectsToChemicalComposition()
        {
            var model = new ShipmentInfoViewModel();
            var result = await shipmentController.Index(model, false) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "ChemicalComposition", "WasteType");
        }
    }
}
