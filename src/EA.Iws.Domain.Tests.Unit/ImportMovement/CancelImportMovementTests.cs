namespace EA.Iws.Domain.Tests.Unit.ImportMovement
{
    using System;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain.ImportMovement;
    using FakeItEasy;
    using Xunit;

    public class CancelImportMovementTests
    {
        private readonly CancelImportMovement cancelMovement;
        private readonly IImportMovementCompletedReceiptRepository completedReceiptRepository;
        private readonly ImportMovement movement;
        private readonly Guid movementId = new Guid("2237DAFC-FC7F-4D80-AA9C-7CE5FA92A7EC");
        private readonly IImportMovementRepository movementRepository;
        private readonly Guid notificationId = new Guid("9613008E-F196-4173-BFF1-79834EE07BB8");
        private readonly IImportMovementReceiptRepository receiptRepository;

        public CancelImportMovementTests()
        {
            completedReceiptRepository = A.Fake<IImportMovementCompletedReceiptRepository>();
            movementRepository = A.Fake<IImportMovementRepository>();
            receiptRepository = A.Fake<IImportMovementReceiptRepository>();
            movement = new ImportMovement(notificationId, 1, new DateTime(2016, 1, 1));

            A.CallTo(() => movementRepository.Get(movementId)).Returns(movement);

            cancelMovement = new CancelImportMovement(movementRepository, receiptRepository, completedReceiptRepository);
        }

        [Fact]
        public async Task CanCancelPrenotifiedMovement()
        {
            A.CallTo(() => completedReceiptRepository.GetByMovementIdOrDefault(movementId))
                .Returns<ImportMovementCompletedReceipt>(null);
            A.CallTo(() => receiptRepository.GetByMovementIdOrDefault(movementId))
                .Returns<ImportMovementReceipt>(null);

            await cancelMovement.Cancel(movementId);

            Assert.True(movement.IsCancelled);
        }

        [Fact]
        public async Task CantCancelReceivedMovement()
        {
            A.CallTo(() => completedReceiptRepository.GetByMovementIdOrDefault(movementId))
                .Returns<ImportMovementCompletedReceipt>(null);
            A.CallTo(() => receiptRepository.GetByMovementIdOrDefault(movementId))
                .Returns(new ImportMovementReceipt(movementId, new ShipmentQuantity(1, ShipmentQuantityUnits.CubicMetres), 
                    new DateTime(2016, 2, 1)));

            Func<Task> testCode = () => cancelMovement.Cancel(movementId);

            await Assert.ThrowsAsync<InvalidOperationException>(testCode);
        }

        [Fact]
        public async Task CantCancelCompletedMovement()
        {
            A.CallTo(() => completedReceiptRepository.GetByMovementIdOrDefault(movementId))
                .Returns(new ImportMovementCompletedReceipt(movementId, new DateTime(2016, 2, 1)));
            A.CallTo(() => receiptRepository.GetByMovementIdOrDefault(movementId))
                .Returns<ImportMovementReceipt>(null);

            Func<Task> testCode = () => cancelMovement.Cancel(movementId);

            await Assert.ThrowsAsync<InvalidOperationException>(testCode);
        }
    }
}