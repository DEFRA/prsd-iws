namespace EA.Iws.Web.Tests.Unit.Controllers.Movement
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Areas.Movement.Controllers;
    using Areas.Movement.ViewModels;
    using FakeItEasy;
    using Requests.Movement;
    using Xunit;

    public class ShipmentDateControllerTests
    {
        private readonly IIwsClient client;
        private readonly ShipmentDateController controller;

        public ShipmentDateControllerTests()
        {
            client = A.Fake<IIwsClient>();
            controller = new ShipmentDateController(() => client);
        }

        [Fact]
        public async Task Index_SendsRequest()
        {
            await controller.Index(Guid.Empty);

            A.CallTo(() => client.SendAsync(A<string>.Ignored, A<GetShipmentDateDataByMovementId>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Create_RedirectsToQuantity()
        {
            var viewModel = new ShipmentDateViewModel();

            var result = await controller.Index(viewModel) as RedirectToRouteResult;

            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("Quantity", result.RouteValues["controller"]);
        }

        [Fact]
        public async Task Create_SendsRequestWithCorrectId()
        {
            await controller.Index(Guid.Empty);

            A.CallTo(() => client.SendAsync(A<string>.Ignored, A<GetShipmentDateDataByMovementId>.That.Matches(r => r.MovementId == Guid.Empty))).MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}