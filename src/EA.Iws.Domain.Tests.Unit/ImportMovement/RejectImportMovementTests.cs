namespace EA.Iws.Domain.Tests.Unit.ImportMovement
{
    using Domain.ImportMovement;
    using EA.Prsd.Core;
    using FakeItEasy;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class RejectImportMovementTests : IDisposable
    {
        private static readonly Guid notificationId = new Guid("675287E3-42D3-4A58-86D4-691ECF620671");
        private static readonly Guid movementId = new Guid("DD7A435F-BB74-4EA6-8680-C1811C46500A");
        private static readonly DateTime Today = new DateTime(2017, 07, 19);
        private static readonly DateTime PastDate = Today.AddDays(-2);
        private static readonly DateTime FutureDate = Today.AddDays(2);
        private readonly IImportMovementRepository movementRepository;
        private readonly IImportMovementRejectionRepository rejectionRepository;
        private readonly IRejectImportMovement rejectFactory;
        private readonly String rejectionreason = "Rejection Date validation tests";
        private ImportMovement movement;
        
        public RejectImportMovementTests()
        {
            SystemTime.Freeze(Today);
            movementRepository = A.Fake<IImportMovementRepository>();
            rejectionRepository = A.Fake<IImportMovementRejectionRepository>();

            rejectFactory = new RejectImportMovement(movementRepository, rejectionRepository);
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }

        [Fact]
        public async Task WasteReceivedDateCanBeInThePast()
        {
            movement = new ImportMovement(notificationId, 52, PastDate);

            A.CallTo(() => movementRepository.Get(movementId)).Returns(movement);

            var result = await rejectFactory.Reject(movementId, PastDate, rejectionreason);

            Assert.Equal(PastDate, result.Date);
        }

        [Fact]
        public async Task WasteReceivedDateCanBeToday()
        {
            movement = new ImportMovement(notificationId, 52, PastDate);

            A.CallTo(() => movementRepository.Get(movementId)).Returns(movement);

            var result = await rejectFactory.Reject(movementId, Today, rejectionreason);

            Assert.Equal(Today, result.Date);
        }

        [Fact]
        public async Task WasteReceivedDateBeforeActualShipmentDate_Throws()
        {
            movement = new ImportMovement(notificationId, 52, FutureDate);

            A.CallTo(() => movementRepository.Get(movementId)).Returns(movement);

            await Assert.ThrowsAsync<InvalidOperationException>(() => rejectFactory.Reject(movementId, PastDate, rejectionreason));
        }

        [Fact]
        public async Task WasteReceivedDateSameasActualShipmentDate()
        {
            movement = new ImportMovement(notificationId, 52, PastDate);

            A.CallTo(() => movementRepository.Get(movementId)).Returns(movement);

            var result = await rejectFactory.Reject(movementId, PastDate, rejectionreason);

            Assert.Equal(PastDate, result.Date);
        }

        [Fact]
        public async Task WasteReceivedDateInfuture_Throws()
        {
            movement = new ImportMovement(notificationId, 52, Today);

            A.CallTo(() => movementRepository.Get(movementId)).Returns(movement);

            await Assert.ThrowsAsync<InvalidOperationException>(() => rejectFactory.Reject(movementId, FutureDate, rejectionreason));
        }
    }
}
