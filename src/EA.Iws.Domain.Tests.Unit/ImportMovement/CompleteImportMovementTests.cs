namespace EA.Iws.Domain.Tests.Unit.ImportMovement
{
    using Core.Shared;
    using Domain.ImportMovement;
    using EA.Prsd.Core;
    using FakeItEasy;
    using System;
    using System.Threading.Tasks;
    using Xunit;
    public class CompleteImportMovementTests : IDisposable
    {
        private static readonly Guid notificationId = new Guid("675287E3-42D3-4A58-86D4-691ECF620671");
        private static readonly Guid movementId = new Guid("DD7A435F-BB74-4EA6-8680-C1811C46500A");
        private static readonly DateTime Today = new DateTime(2017, 07, 19);
        private static readonly DateTime PastDate = Today.AddDays(-2);
        private static readonly DateTime FutureDate = Today.AddDays(2);
        private readonly IImportMovementRepository movementRepository;
        private readonly IImportMovementReceiptRepository receiptRepository;
        private readonly IImportMovementCompletedReceiptRepository completedReceiptRepository;
        private readonly ICompleteImportMovement completeFactory;
        private ImportMovement movement;
        private ImportMovementReceipt movementReceipt;

        public CompleteImportMovementTests()
        {
            SystemTime.Freeze(Today);
            movementRepository = A.Fake<IImportMovementRepository>();
            receiptRepository = A.Fake<IImportMovementReceiptRepository>();
            completedReceiptRepository = A.Fake<IImportMovementCompletedReceiptRepository>();
            completeFactory = new CompleteImportMovement(movementRepository, completedReceiptRepository, receiptRepository);
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }

        [Fact]
        public async Task WasteReceivedDateCanBeInThePast()
        {
            movement = new ImportMovement(notificationId, 52, PastDate);

            movementReceipt = new ImportMovementReceipt(movementId, new ShipmentQuantity(10, ShipmentQuantityUnits.Kilograms), PastDate);

            A.CallTo(() => movementRepository.Get(movementId)).Returns(movement);
            A.CallTo(() => receiptRepository.GetByMovementIdOrDefault(movementId)).Returns(movementReceipt);

            var result = await completeFactory.Complete(movementId,  PastDate);

            Assert.Equal(PastDate, result.Date);
        }

        [Fact]
        public async Task WasteReceivedDateCanBeToday()
        {
            movement = new ImportMovement(notificationId, 52, PastDate);

            movementReceipt = new ImportMovementReceipt(movementId, new ShipmentQuantity(10, ShipmentQuantityUnits.Kilograms), PastDate);

            A.CallTo(() => movementRepository.Get(movementId)).Returns(movement);
            A.CallTo(() => receiptRepository.GetByMovementIdOrDefault(movementId)).Returns(movementReceipt);

            var result = await completeFactory.Complete(movementId,  Today);

            Assert.Equal(Today, result.Date);
        }

        [Fact]
        public async Task WasteReceivedDateBeforeActualShipmentDate_Throws()
        {
            movement = new ImportMovement(notificationId, 52, FutureDate);

            movementReceipt = new ImportMovementReceipt(movementId, new ShipmentQuantity(10, ShipmentQuantityUnits.Kilograms), FutureDate);

            A.CallTo(() => movementRepository.Get(movementId)).Returns(movement);

            A.CallTo(() => receiptRepository.GetByMovementIdOrDefault(movementId)).Returns(movementReceipt);
            
            await Assert.ThrowsAsync<InvalidOperationException>(() => completeFactory.Complete(movementId,  PastDate));
        }

        [Fact]
        public async Task WasteReceivedDateSameasActualShipmentDate()
        {
            movement = new ImportMovement(notificationId, 52, PastDate);

            movementReceipt = new ImportMovementReceipt(movementId, new ShipmentQuantity(10, ShipmentQuantityUnits.Kilograms), PastDate);

            A.CallTo(() => movementRepository.Get(movementId)).Returns(movement);

            A.CallTo(() => receiptRepository.GetByMovementIdOrDefault(movementId)).Returns(movementReceipt);
            
            var result = await completeFactory.Complete(movementId,  PastDate);

            Assert.Equal(PastDate, result.Date);
        }

        [Fact]
        public async Task WasteReceivedDateInfuture_Throws()
        {
            movement = new ImportMovement(notificationId, 52, Today);

            movementReceipt = new ImportMovementReceipt(movementId, new ShipmentQuantity(10, ShipmentQuantityUnits.Kilograms), Today);
            
            A.CallTo(() => movementRepository.Get(movementId)).Returns(movement);

            A.CallTo(() => receiptRepository.GetByMovementIdOrDefault(movementId)).Returns(movementReceipt);

            await Assert.ThrowsAsync<InvalidOperationException>(() => completeFactory.Complete(movementId,  FutureDate));
        }
    }
}
