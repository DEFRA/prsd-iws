namespace EA.Iws.Web.Tests.Unit.Controllers.Movement
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.Movement.Controllers;
    using Areas.Movement.ViewModels.Quantity;
    using Core.MovementReceipt;
    using Core.Shared;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.MovementReceipt;
    using Xunit;

    public class QuantityReceivedControllerTests
    {
        private static readonly Guid AnyGuid = new Guid("5AD21761-507B-4A19-8763-709AC3C5813E");

        private readonly QuantityReceivedController controller;
        private readonly IMediator mediator;
        private readonly QuantityReceivedViewModel viewModel;

        public QuantityReceivedControllerTests()
        {
            mediator = A.Fake<IMediator>();
            controller = new QuantityReceivedController(mediator);

            A.CallTo(() =>
                mediator.SendAsync(A<GetMovementReceiptQuantityByMovementId>.That.Matches(r => r.Id == AnyGuid)))
                .Returns(new MovementReceiptQuantityData
                {
                    Unit = ShipmentQuantityUnits.Kilograms,
                    Quantity = 250
                });

            viewModel = new QuantityReceivedViewModel
            {
                Unit = ShipmentQuantityUnits.Kilograms,
                Quantity = 250
            };
        }

        [Fact]
        public async Task Get_SendsCorrectQuery()
        {
            await controller.Index(AnyGuid);

            A.CallTo(() =>
                mediator.SendAsync(A<GetMovementReceiptQuantityByMovementId>.That.Matches(r => r.Id == AnyGuid)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Get_ReturnsCorrectModel()
        {
            var result = await controller.Index(AnyGuid) as ViewResult;

            Assert.NotNull(result);
            var model = Assert.IsType<QuantityReceivedViewModel>(result.Model);
            Assert.Equal(ShipmentQuantityUnits.Kilograms, model.Unit);
            Assert.Equal(250, model.Quantity);
        }

        [Fact]
        public async Task PostWithInvalidModel_ReturnsView()
        {
            controller.ModelState.AddModelError("a", "b");

            var result = await controller.Index(AnyGuid, viewModel) as ViewResult;

            Assert.NotNull(result);
            var modelReturned = Assert.IsType<QuantityReceivedViewModel>(result.Model);
            Assert.Equal(viewModel, modelReturned);
        }

        [Fact]
        public async Task Post_SendsCorrectCommand()
        {
            await controller.Index(AnyGuid, viewModel);

            A.CallTo(() => mediator.SendAsync(A<SetMovementReceiptQuantityByMovementId>
                    .That.Matches(r => r.Id == AnyGuid
                                  && r.Quantity == viewModel.Quantity)))
                    .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Post_RedirectsToCorrectAction()
        {
            var result = await controller.Index(AnyGuid, viewModel) as RedirectToRouteResult;

            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("ReceiptComplete", result.RouteValues["controller"]);
            Assert.Equal(AnyGuid, result.RouteValues["id"]);
        }
    }
}
