namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using System.Threading.Tasks;
    using Core.MovementReceipt;
    using Prsd.Core;
    using RequestHandlers.Mappings.Movement;
    using RequestHandlers.Movement;
    using Requests.Movement;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class GetActiveMovementsWithoutReceiptCertificateByNotificationIdHandlerTests : TestBase, IDisposable
    {
        private readonly GetActiveMovementsWithoutReceiptCertificateByNotificationIdHandler handler;
        private readonly GetActiveMovementsWithoutReceiptCertificateByNotificationId request;

        private readonly TestableMovement notificationMovement;

        public GetActiveMovementsWithoutReceiptCertificateByNotificationIdHandlerTests()
        {
            handler = new GetActiveMovementsWithoutReceiptCertificateByNotificationIdHandler(Context, new MovementMap());
            SystemTime.Freeze(MiddleDate);

            notificationMovement = new TestableMovement
            {
                Id = MovementId,
                Number = TestInt,
                NotificationApplication = NotificationApplication,
                NotificationApplicationId = NotificationId,
                Date = OldestDate
            };

            request = new GetActiveMovementsWithoutReceiptCertificateByNotificationId(NotificationId);
        }

        [Fact]
        public async Task NotificationHasNoMovements_ReturnsEmpty()
        {
            var result = await handler.HandleAsync(request);

            Assert.Empty(result);
        }

        [Fact]
        public async Task NotificationHasFutureMovement_ReturnsEmpty()
        {
            notificationMovement.Date = NewestDate;
            Context.Movements.Add(notificationMovement);

            var result = await handler.HandleAsync(request);

            Assert.Empty(result);
        }

        [Fact]
        public async Task NotificationHasActiveMovementWithoutReceiptCertificate_ReturnsMovement()
        {
            Context.Movements.Add(notificationMovement);

            var result = await handler.HandleAsync(request);

            Assert.Single(result, m => m.Id == MovementId 
                && m.Number == TestInt);
        }

        [Fact]
        public async Task NotificationHasActiveMovementWithCertificateOfReceipt_ReturnsResult()
        {
            notificationMovement.Receipt = new TestableMovementReceipt
            {
                Decision = Decision.Accepted
            };

            Context.Movements.Add(notificationMovement);

            var result =
                await
                    handler.HandleAsync(request);

            Assert.Single(result, m => m.Id == MovementId
                && m.Number == TestInt);
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }
    }
}
