namespace EA.Iws.Domain.Tests.Unit.ImportMovement
{
    using Domain.ImportMovement;
    using EA.Iws.Core.Shared;
    using EA.Prsd.Core;
    using FakeItEasy;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class PartialRejectImportMovementTests : IDisposable
    {
        private static readonly Guid notificationId = new Guid("675287E3-42D3-4A58-86D4-691ECF620671");
        private static readonly Guid movementId = new Guid("DD7A435F-BB74-4EA6-8680-C1811C46500A");
        private static readonly DateTime Today = new DateTime(2017, 07, 19);
        private static readonly DateTime PastDate = Today.AddDays(-2);
        private static readonly DateTime FutureDate = Today.AddDays(2);
        private readonly IImportMovementRepository movementRepository;
        private readonly IImportMovementPartailRejectionRepository partialRejectionRepository;
        private readonly IPartialRejectionImportMovement partialRejectFactory;
        private readonly string rejectionreason = "Rejection Date validation tests";
        private ImportMovement movement;
        private readonly ShipmentQuantityUnits shipmentQuantityUnits = ShipmentQuantityUnits.Tonnes;

        public PartialRejectImportMovementTests()
        {
            SystemTime.Freeze(Today);
            movementRepository = A.Fake<IImportMovementRepository>();
            partialRejectionRepository = A.Fake<IImportMovementPartailRejectionRepository>();

            partialRejectFactory = new PartialRejectionImportMovement(movementRepository, partialRejectionRepository);
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

            var result = await partialRejectFactory.PartailReject(movementId, PastDate, rejectionreason, 15, shipmentQuantityUnits, 5, shipmentQuantityUnits, PastDate);

            Assert.Equal(PastDate, result.WasteReceivedDate);
        }

        [Fact]
        public async Task WasteReceivedDateCanBeToday()
        {
            movement = new ImportMovement(notificationId, 52, PastDate);

            A.CallTo(() => movementRepository.Get(movementId)).Returns(movement);

            var result = await partialRejectFactory.PartailReject(movementId, Today, rejectionreason, 15, shipmentQuantityUnits, 5, shipmentQuantityUnits, Today);

            Assert.Equal(Today, result.WasteReceivedDate);
        }

        [Fact]
        public async Task WasteReceivedDateSameasActualShipmentDate()
        {
            movement = new ImportMovement(notificationId, 52, PastDate);

            A.CallTo(() => movementRepository.Get(movementId)).Returns(movement);

            var result = await partialRejectFactory.PartailReject(movementId, PastDate, rejectionreason, 15, shipmentQuantityUnits, 5, shipmentQuantityUnits, PastDate);

            Assert.Equal(PastDate, result.WasteReceivedDate);
        }

        [Fact]
        public async Task WasteReceivedDateInfuture_Throws()
        {
            movement = new ImportMovement(notificationId, 52, Today);

            A.CallTo(() => movementRepository.Get(movementId)).Returns(movement);

            await Assert.ThrowsAsync<InvalidOperationException>(() => partialRejectFactory.PartailReject(movementId, FutureDate, rejectionreason, 15, shipmentQuantityUnits, 5, shipmentQuantityUnits, PastDate));
        }

        [Fact]
        public async Task RejectedQtyCanNotBeGreaterThanActualQty_Throws()
        {
            movement = new ImportMovement(notificationId, 52, FutureDate);

            A.CallTo(() => movementRepository.Get(movementId)).Returns(movement);

            await Assert.ThrowsAsync<InvalidOperationException>(() => partialRejectFactory.PartailReject(movementId, PastDate, rejectionreason, 15, shipmentQuantityUnits, 25, shipmentQuantityUnits, PastDate));
        }

        [Fact]
        public async Task RejectionReasonCanNotBeNullOrEmpty_Throws()
        {
            movement = new ImportMovement(notificationId, 52, FutureDate);

            A.CallTo(() => movementRepository.Get(movementId)).Returns(movement);

            await Assert.ThrowsAsync<InvalidOperationException>(() => partialRejectFactory.PartailReject(movementId, PastDate, string.Empty, 15, shipmentQuantityUnits, 25, shipmentQuantityUnits, PastDate));
        }
    }
}
