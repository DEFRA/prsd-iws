namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using Core.Movement;
    using Domain.Movement;
    using EA.Iws.Core.Shared;
    using FakeItEasy;
    using Prsd.Core;
    using System;
    using System.Threading.Tasks;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class PartialRejectMovementTests
    {
        private static readonly Guid NotificationId = new Guid("675287E3-42D3-4A58-86D4-691ECF620671");
        private static readonly Guid MovementId = new Guid("DD7A435F-BB74-4EA6-8680-C1811C46500A");
        private static readonly DateTime Today = new DateTime(2017, 07, 19);
        private static readonly DateTime PastDate = Today.AddDays(-2);
        private static readonly DateTime FutureDate = Today.AddDays(2);
        private readonly IMovementRepository movementRepository;
        private readonly IMovementPartialRejectionRepository movementPartialRejectionRepository;
        private readonly IPartialRejectionMovement partialRejectionFactory;
        private readonly String rejectionreason = "Rejection Date validation tests";
        private Movement movement;

        public PartialRejectMovementTests()
        {
            SystemTime.Freeze(Today);
            movementRepository = A.Fake<IMovementRepository>();
            movementPartialRejectionRepository = A.Fake<IMovementPartialRejectionRepository>();

            partialRejectionFactory = new PartialRejectionMovement(movementRepository, movementPartialRejectionRepository);
        }

        [Fact]
        public void Dispose()
        {
            SystemTime.Unfreeze();
        }

        [Fact]
        public async Task WasteReceivedDateCanBeInThePast()
        {
            movement = new TestableMovement
            {
                Id = MovementId,
                Date = PastDate,
                NotificationId = NotificationId,
                Status = MovementStatus.Captured
            };

            A.CallTo(() => movementRepository.GetById(MovementId)).Returns(movement);

            var result = await partialRejectionFactory.PartailReject(movement.Id, PastDate, rejectionreason, 1, ShipmentQuantityUnits.Tonnes, 10, ShipmentQuantityUnits.Tonnes, PastDate);

            Assert.Equal(PastDate, result.WasteReceivedDate);
        }

        [Fact]
        public async Task WasteReceivedDateCanBeToday()
        {
            movement = new TestableMovement
            {
                Id = MovementId,
                Date = PastDate,
                NotificationId = NotificationId,
                Status = MovementStatus.Captured
            };

            A.CallTo(() => movementRepository.GetById(MovementId)).Returns(movement);

            var result = await partialRejectionFactory.PartailReject(movement.Id, Today, rejectionreason, 1, ShipmentQuantityUnits.Tonnes, 10, ShipmentQuantityUnits.Tonnes, Today);

            Assert.Equal(Today, result.WasteReceivedDate);
        }

        [Fact]
        public async Task WasteReceivedDateSameasActualShipmentDate()
        {
            movement = new TestableMovement
            {
                Id = MovementId,
                Date = PastDate,
                NotificationId = NotificationId,
                Status = MovementStatus.Captured
            };

            A.CallTo(() => movementRepository.GetById(MovementId)).Returns(movement);

            var result = await partialRejectionFactory.PartailReject(movement.Id, PastDate, rejectionreason, 1, ShipmentQuantityUnits.Tonnes, 10, ShipmentQuantityUnits.Tonnes, PastDate);

            Assert.Equal(PastDate, result.WasteReceivedDate);
        }

        [Fact]
        public async Task WasteReceivedDateInfuture_Throws()
        {
            movement = new TestableMovement
            {
                Id = MovementId,
                Date = Today,
                NotificationId = NotificationId
            };

            A.CallTo(() => movementRepository.GetById(MovementId)).Returns(movement);

            await Assert.ThrowsAsync<InvalidOperationException>(() => partialRejectionFactory.PartailReject(movement.Id, Today, rejectionreason, 1, ShipmentQuantityUnits.Tonnes, 10, ShipmentQuantityUnits.Tonnes, FutureDate));
        }
    }
}
