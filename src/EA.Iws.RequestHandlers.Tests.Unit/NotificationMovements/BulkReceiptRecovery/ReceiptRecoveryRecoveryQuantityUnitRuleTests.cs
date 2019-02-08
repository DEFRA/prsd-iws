namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkReceiptRecovery
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Rules;
    using Core.Shared;
    using Domain;
    using Domain.Movement;
    using Domain.NotificationApplication.Shipment;
    using FakeItEasy;
    using RequestHandlers.NotificationMovements.BulkReceiptRecovery;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class ReceiptRecoveryRecoveryQuantityUnitRuleTests
    {
        private readonly ReceiptRecoveryRecoveryQuantityUnitRule rule;
        private readonly Guid notificationId;
        private readonly IShipmentInfoRepository shipmentInfoRepository;

        public ReceiptRecoveryRecoveryQuantityUnitRuleTests()
        {
            shipmentInfoRepository = A.Fake<IShipmentInfoRepository>();
            rule = new ReceiptRecoveryRecoveryQuantityUnitRule(shipmentInfoRepository);
            notificationId = Guid.NewGuid();
        }

        [Fact]
        public async Task QuantityIsAccepted_Success()
        {
            A.CallTo(() => shipmentInfoRepository.GetByNotificationId(notificationId)).Returns(GetRepoData(true));
            var result = await rule.GetResult(GetTestData(true), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.QuantityUnit, result.Rule);
            Assert.Equal(MessageLevel.Success, result.MessageLevel);
        }

        [Fact]
        public async Task QuantityNotAccepted_Failure()
        {
            A.CallTo(() => shipmentInfoRepository.GetByNotificationId(notificationId)).Returns(GetRepoData(false));
            var result = await rule.GetResult(GetTestData(false), notificationId);

            Assert.Equal(ReceiptRecoveryContentRules.QuantityUnit, result.Rule);
            Assert.Equal(MessageLevel.Error, result.MessageLevel);
        }

        private List<ReceiptRecoveryMovement> GetTestData(bool correctUnit)
        {
            return new List<ReceiptRecoveryMovement>()
            {
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = 1,
                    Unit = correctUnit ? ShipmentQuantityUnits.Kilograms : ShipmentQuantityUnits.CubicMetres
                },
                new ReceiptRecoveryMovement()
                {
                    ShipmentNumber = 2,
                    Unit = ShipmentQuantityUnits.Kilograms
                }
            };
        }
        private ShipmentInfo GetRepoData(bool correctUnit)
        {
            var a = new ShipmentInfo(notificationId, new ShipmentPeriod(DateTime.Now, DateTime.Now, false), 10, new ShipmentQuantity(10, ShipmentQuantityUnits.Kilograms));
            a.UpdateQuantity(new ShipmentQuantity(10, correctUnit ? ShipmentQuantityUnits.Kilograms : ShipmentQuantityUnits.CubicMetres));
            return a;
        }
    }
}
