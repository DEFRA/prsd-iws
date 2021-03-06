﻿namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkPrenotification
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.BulkPrenotification;
    using Core.Rules;
    using Domain.Movement;
    using FakeItEasy;
    using RequestHandlers.NotificationMovements.BulkPrenotification;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class PrenotificationInvalidShipmentNumbersRuleTests
    {
        private readonly IMovementRepository repo;
        private readonly Guid notificationId = new Guid("DD1F019D-BD85-4A6F-89AB-328A7BD53CEA");

        private PrenotificationOnlyNewShipmentsRule rule;

        public PrenotificationInvalidShipmentNumbersRuleTests()
        {
            this.repo = A.Fake<IMovementRepository>();

            var testMovements = new List<Movement>()
            {
                new TestableMovement()
                {
                    Number = 1
                },
                new TestableMovement()
                {
                    Number = 2
                }
            };

            A.CallTo(() => repo.GetAllMovements(notificationId)).Returns(testMovements);
        }

        [Fact]
        public async Task ShipmentNumberGreaterThanExistingShipmentNumber()
        {
            rule = new PrenotificationOnlyNewShipmentsRule(repo);

            var movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                   ShipmentNumber = 3
                }
            };

            var result = await rule.GetResult(movements, notificationId);

            Assert.Equal(PrenotificationContentRules.OnlyNewShipments, result.Rule);
            Assert.Equal(MessageLevel.Success.ToString(), result.MessageLevel.ToString());
        }

        [Fact]
        public async Task ShipmentNumberLessThanExistingShipmentNumber()
        {
            rule = new PrenotificationOnlyNewShipmentsRule(repo);

            var movements = new List<PrenotificationMovement>()
            {
                new PrenotificationMovement()
                {
                   ShipmentNumber = 1
                }
            };

            var result = await rule.GetResult(movements, notificationId);

            Assert.Equal(MessageLevel.Error.ToString(), result.MessageLevel.ToString());
        }
    }
}
