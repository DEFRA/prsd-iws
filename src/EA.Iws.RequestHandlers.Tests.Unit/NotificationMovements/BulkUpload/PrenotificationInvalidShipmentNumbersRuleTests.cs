namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkUpload
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.Bulk;
    using Core.Rules;
    using Domain.Movement;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using RequestHandlers.NotificationMovements.BulkUpload;
    using Xunit;

    public class PrenotificationInvalidShipmentNumbersRuleTests
    {
        private readonly IMediator mediator;
        private readonly IMovementRepository repo;
        private readonly Guid notificationId = new Guid("DD1F019D-BD85-4A6F-89AB-328A7BD53CEA");

        private PrenotificationContentOnlyNewShipmentsRule rule;

        public PrenotificationInvalidShipmentNumbersRuleTests()
        {
            this.mediator = A.Fake<IMediator>();
            this.repo = A.Fake<IMovementRepository>();

            A.CallTo(() => repo.GetAllMovements(notificationId)).Returns(A.CollectionOfFake<Movement>(2));
        }

        [Fact]
        public async Task ShipmentNumberGreaterThanExistingShipmentNumber()
        {
            rule = new PrenotificationContentOnlyNewShipmentsRule(repo);

            List<PrenotificationMovement> movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                   ShipmentNumber = 3
                }
            };

            var result = await rule.GetResult(movements, notificationId);

            Assert.Equal(MessageLevel.Success.ToString(), result.MessageLevel.ToString());
        }

        [Fact]
        public async Task ShipmentNumberLessThanExistingShipmentNumber()
        {
            rule = new PrenotificationContentOnlyNewShipmentsRule(repo);

            List<PrenotificationMovement> movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                   ShipmentNumber = 1
                }
            };

            var result = await rule.GetResult(movements, notificationId);

            Assert.Equal(MessageLevel.Error.ToString(), result.MessageLevel.ToString());
            Assert.Equal("Shipment number 1: this shipment number already exists.", result.ErrorMessage);
        }
    }
}
