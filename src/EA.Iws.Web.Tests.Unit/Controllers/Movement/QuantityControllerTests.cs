namespace EA.Iws.Web.Tests.Unit.Controllers.Movement
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.Movement.Controllers;
    using Areas.Movement.ViewModels.Quantity;
    using Core.Shared;
    using FakeItEasy;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Movement;
    using Xunit;

    public class QuantityControllerTests
    {
        private static readonly Guid AnyGuid = new Guid("5AD21761-507B-4A19-8763-709AC3C5813E");

        private readonly QuantityController controller;
        private readonly IMediator mediator;
        private readonly TestMap mapper;

        public QuantityControllerTests()
        {
            mediator = A.Fake<IMediator>();
            mapper = new TestMap();

            A.CallTo(() => mediator.SendAsync(A<GetMovementQuantityDataByMovementId>.Ignored))
                .Returns(new MovementQuantityData());

            controller = new QuantityController(mediator, mapper);
        }

        [Fact]
        public async Task GetSendsRequest()
        {
            await controller.Index(AnyGuid);

            A.CallTo(() => mediator.SendAsync(A<GetMovementQuantityDataByMovementId>.That.Matches(r => r.MovementId == AnyGuid)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task GetMapsResult()
        {
            await controller.Index(AnyGuid);

            Assert.True(mapper.WasMapCalled);
        }

        [Fact]
        public async Task GetReturnsViewWithCorrectViewModel()
        {
            var result = await controller.Index(AnyGuid) as ViewResult;

            var model = result.Model as QuantityViewModel;

            Assert.NotNull(model);
            Assert.Equal(100, model.TotalNotified);
        }

        [Fact]
        public async Task PostInvalidModelStateDoesNotSendRequest()
        {
            controller.ModelState.AddModelError("Error", "Error");

            await controller.Index(AnyGuid, new QuantityViewModel());

            A.CallTo(() => mediator.SendAsync(A<GetMovementQuantityDataByMovementId>.Ignored))
                .MustNotHaveHappened();
        }

        [Fact]
        public async Task PostInvalidModelStateReturnsSameModel()
        {
            controller.ModelState.AddModelError("Error", "Error");

            var model = new QuantityViewModel
            {
                Units = ShipmentQuantityUnits.Kilograms,
                TotalAvailable = 100,
                TotalNotified = 100
            };

            var result = await controller.Index(AnyGuid, model) as ViewResult;

            Assert.Equal(model, result.Model);
        }

        [Fact]
        public async Task PostSendsRequest()
        {
            var model = new QuantityViewModel
            {
                Units = ShipmentQuantityUnits.Litres,
                Quantity = "100"
            };

            await controller.Index(AnyGuid, model);

            A.CallTo(() => mediator.SendAsync(A<SetMovementQuantityByMovementId>.That.Matches(r => r.Id == AnyGuid
                        && r.Units == model.Units
                        && r.Quantity == Convert.ToDecimal(model.Quantity))))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task PostRedirectsToPackagingTypes()
        {
            var result = await controller.Index(AnyGuid, new QuantityViewModel
            {
                Units = ShipmentQuantityUnits.Litres,
                Quantity = "100"
            }) as RedirectToRouteResult;

            Assert.Equal("Index", result.RouteValues["action"]);
            Assert.Equal("PackagingTypes", result.RouteValues["controller"]);
            Assert.Equal(AnyGuid, result.RouteValues["id"]);
        }

        private class TestMap : IMap<MovementQuantityData, QuantityViewModel>
        {
            public bool WasMapCalled { get; private set; }

            public QuantityViewModel Map(MovementQuantityData source)
            {
                WasMapCalled = true;
                return new QuantityViewModel
                {
                    TotalAvailable = 50,
                    TotalUsed = 50,
                    TotalNotified = 100,
                    Units = ShipmentQuantityUnits.CubicMetres,
                    Quantity = null
                };
            }
        }
    }
}
