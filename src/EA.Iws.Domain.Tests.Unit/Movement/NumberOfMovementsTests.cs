namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Movement;
    using Domain.NotificationApplication.Shipment;
    using FakeItEasy;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class NumberOfMovementsTests
    {
        private static readonly Guid NotificationId = new Guid("0E38E99F-A997-4014-8438-62B56E0398DF");
        private readonly IMovementRepository movementRepository;
        private readonly NumberOfMovements numberOfMovements;
        private readonly IShipmentInfoRepository shipmentRepository;

        public NumberOfMovementsTests()
        {
            shipmentRepository = A.Fake<IShipmentInfoRepository>();
            movementRepository = A.Fake<IMovementRepository>();

            numberOfMovements = new NumberOfMovements(movementRepository, shipmentRepository);
        }

        [Fact]
        public async Task TotalMovementsReached_ReturnsTrue()
        {
            var shipment = CreateShipmentInfo(1);

            A.CallTo(() => movementRepository.GetAllMovements(NotificationId)).Returns(GetMovements(1));
            A.CallTo(() => shipmentRepository.GetByNotificationId(NotificationId)).Returns(shipment);

            var result = await numberOfMovements.HasMaximum(NotificationId);

            Assert.True(result);
        }

        [Fact]
        public async Task TotalMovementsExceeded_ReturnsTrue()
        {
            var shipment = CreateShipmentInfo(1);

            A.CallTo(() => movementRepository.GetAllMovements(NotificationId)).Returns(GetMovements(2));
            A.CallTo(() => shipmentRepository.GetByNotificationId(NotificationId)).Returns(shipment);

            var result = await numberOfMovements.HasMaximum(NotificationId);

            Assert.True(result);
        }

        [Fact]
        public async Task TotalMovementsNotReached_ReturnsFalse()
        {
            var shipment = CreateShipmentInfo(2);

            A.CallTo(() => movementRepository.GetAllMovements(NotificationId)).Returns(GetMovements(1));
            A.CallTo(() => shipmentRepository.GetByNotificationId(NotificationId)).Returns(shipment);

            var result = await numberOfMovements.HasMaximum(NotificationId);

            Assert.False(result);
        }

        private static TestableShipmentInfo CreateShipmentInfo(int numberOfShipments)
        {
            return new TestableShipmentInfo
            {
                Id = new Guid("2DA8E281-A6A4-459A-A38A-B4B0643E0726"),
                NotificationId = NotificationId,
                NumberOfShipments = numberOfShipments
            };
        }

        private static IEnumerable<TestableMovement> GetMovements(int numberOfMovements)
        {
            for (int i = 0; i < numberOfMovements; i++)
            {
                yield return new TestableMovement
                {
                    Id = Guid.NewGuid(),
                    NotificationId = NotificationId
                };
            }
        }
    }
}