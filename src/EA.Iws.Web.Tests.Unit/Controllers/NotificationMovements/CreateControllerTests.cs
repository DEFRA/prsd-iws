namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationMovements
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.NotificationMovements.Controllers;
    using Core.Movement;
    using Core.PackagingType;
    using Core.Rules;
    using Core.Shared;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements;
    using Requests.NotificationMovements.Create;
    using Xunit;

    public class CreateControllerTests
    {
        private readonly IMediator mediator;
        private readonly CreateController controller;
        private readonly MovementRulesSummary movementRulesSummary;

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

        [Fact]
        public async Task RedirectToShipmentDate_Send_Request()
        {
            var result = await controller.RedirectToShipmentDate(Guid.NewGuid()) as ViewResult;

            // Redirect from warning page if Consent expires in 4 working days or less than 3 days
            // should skip the rules.
            A.CallTo(() => mediator.SendAsync(A<GetMovementRulesSummary>.Ignored))
                .MustHaveHappened(Repeated.Never);

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
    }
}
