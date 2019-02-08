namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationMovements
{
    using Areas.NotificationMovements.Controllers;
    using Areas.NotificationMovements.ViewModels.Create;
    using Core.Movement;
    using Core.PackagingType;
    using Core.Rules;
    using Core.Shared;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements;
    using Requests.NotificationMovements.Create;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Web.ViewModels.Shared;
    using Xunit;

    public class CreateControllerTests
    {
        private readonly IMediator mediator;
        private readonly CreateController controller;
        private readonly MovementRulesSummary movementRulesSummary;
        private readonly Guid notificationId = new Guid("4AB23CDF-9B24-4598-A302-A69EBB5F2152");
        private Guid[] movementId = new Guid[1];

        public CreateControllerTests()
        {
            mediator = A.Fake<IMediator>();
            controller = new CreateController(mediator);
            movementRulesSummary = new MovementRulesSummary(new List<RuleResult<MovementRules>>());

            A.CallTo(() => mediator.SendAsync(A<GetMovementRulesSummary>.Ignored)).Returns(movementRulesSummary);
            A.CallTo(() => mediator.SendAsync(A<GetShipmentInfo>.Ignored)).Returns(GetShipmentInfo());
        }

        [Fact]
        public async Task Index_Send_Request()
        {
            var result = await controller.Index(Guid.NewGuid()) as ViewResult;

            A.CallTo(() => mediator.SendAsync(A<GetMovementRulesSummary>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);

            A.CallTo(() => mediator.SendAsync(A<GetShipmentInfo>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);

            Assert.NotNull(result);
            Assert.True(result.ViewName == "Index");
        }

        private static ShipmentInfo GetShipmentInfo()
        {
            var shipmentDates = new ShipmentDates() { StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(6) };
            var packagingData = new PackagingData() { OtherDescription = "Packaging description" };

            return new ShipmentInfo()
            {
                ShipmentDates = shipmentDates,
                PackagingData = packagingData,
                ShipmentQuantityUnits = ShipmentQuantityUnits.Tonnes
            };
        }

        [Fact]
        public async Task Create_ReturnsWhoAreyourCarrier()
        {
            var packagingTypes = CheckBoxCollectionViewModel.CreateFromEnum<PackagingType>();
            var selectedValues = new[] { 1, 2 };
            movementId[0] = new Guid("AF1839A1-DA40-430B-9DFE-D79194175DFD");
            var remainingshipment = new RemainingShipmentsData();
            remainingshipment.ShipmentsRemaining = 400;
            remainingshipment.ActiveLoadsRemaining = 300;
            var model = new CreateMovementsViewModel();
            model.Day = 6;
            model.Month = 2;
            model.Year = 2019;
            model.Quantity = "1";
            model.NumberToCreate = 1;
            model.PackagingTypes = packagingTypes;
            model.PackagingTypes.PossibleValues.First(y => y.Value == "1").Selected = true;
            model.Units = ShipmentQuantityUnits.Kilograms;
            A.CallTo(
              () =>
                  mediator.SendAsync(A<GetRemainingShipments>.That.Matches(p => p.NotificationId == notificationId)))
              .Returns(remainingshipment);

            A.CallTo(
            () =>
                mediator.SendAsync(A<GetWorkingDaysUntil>.That.Matches(p => p.NotificationId == notificationId)))
            .Returns(6);

            A.CallTo(
          () =>
              mediator.SendAsync(A<CreateMovements>.That.Matches(p => p.NotificationId == notificationId)))
          .Returns(movementId);

            var result = await controller.Index(notificationId, model) as RedirectToRouteResult;

            Assert.Equal("WhoAreYourCarriers", result.RouteValues["action"]);
        }
    }
}
