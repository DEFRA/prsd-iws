namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using Core.Movement;
    using Domain.Movement;
    using FakeItEasy;
    using Prsd.Core;
    using System;
    using System.Threading.Tasks;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class RejectMovementTests
    {
        private static readonly Guid NotificationId = new Guid("675287E3-42D3-4A58-86D4-691ECF620671");
        private static readonly Guid MovementId = new Guid("DD7A435F-BB74-4EA6-8680-C1811C46500A");
        private static readonly DateTime Today = new DateTime(2017, 07, 19);
        private static readonly DateTime PastDate = Today.AddDays(-2);
        private static readonly DateTime FutureDate = Today.AddDays(2);
        private readonly IMovementRepository movementRepository;
        private readonly IMovementRejectionRepository movementRejectionRepository;
        private readonly IRejectMovement rejectFactory;
        private readonly String rejectionreason = "Rejection Date validation tests";
        private Movement movement;

        public RejectMovementTests()
        {
            SystemTime.Freeze(Today);
            movementRepository = A.Fake<IMovementRepository>();
            movementRejectionRepository = A.Fake<IMovementRejectionRepository>();

            rejectFactory = new RejectMovement(movementRepository, movementRejectionRepository);
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

           var result = await rejectFactory.Reject(movement.Id, PastDate, rejectionreason);
           
            Assert.Equal(PastDate, result.Date);
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

            var result = await rejectFactory.Reject(movement.Id, Today, rejectionreason);

            Assert.Equal(Today, result.Date);
        }

        [Fact]
        public async Task WasteReceivedDateBeforeActualShipmentDate_Throws()
        {
            movement = new TestableMovement
            {
                Id = MovementId,
                Date = FutureDate,
                NotificationId = NotificationId
            };

            A.CallTo(() => movementRepository.GetById(MovementId)).Returns(movement);

            await Assert.ThrowsAsync<InvalidOperationException>(() => rejectFactory.Reject(movement.Id, PastDate, rejectionreason));
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

            var result = await rejectFactory.Reject(movement.Id, PastDate, rejectionreason);

            Assert.Equal(PastDate, result.Date);
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

            await Assert.ThrowsAsync<InvalidOperationException>(() => rejectFactory.Reject(movement.Id, FutureDate, rejectionreason));
        }
    }
}
