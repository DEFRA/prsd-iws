namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using System.Threading.Tasks;
    using Core.MovementReceipt;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Prsd.Core;
    using RequestHandlers.Mappings.Movement;
    using RequestHandlers.Movement;
    using Requests.Movement;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class GetActiveMovementsWithReceiptCertificateByNotificationIdHandlerTests : TestBase, IDisposable
    {
        private readonly GetActiveMovementsWithReceiptCertificateByNotificationIdHandler handler;
        private readonly GetActiveMovementsWithReceiptCertificateByNotificationId request;

        private readonly TestableMovement notificationMovement;
        private readonly TestableNotificationApplication notificationApplication;

        public GetActiveMovementsWithReceiptCertificateByNotificationIdHandlerTests()
        {
            handler = new GetActiveMovementsWithReceiptCertificateByNotificationIdHandler(Context, new MovementMap(), new ReceivedMovements(new ActiveMovements()));

            SystemTime.Freeze(MiddleDate);

            notificationApplication = new TestableNotificationApplication();

            notificationMovement = new TestableMovement
            {
                Id = MovementId,
                Number = TestInt,
                NotificationId = NotificationId,
                Date = OldestDate
            };

            request = new GetActiveMovementsWithReceiptCertificateByNotificationId(NotificationId);
        }

        [Fact]
        public async Task NotificationHasNoMovements_ReturnsEmpty()
        {
            AddNotification();

            var result = await handler.HandleAsync(request);

            Assert.Empty(result.MovementDatas);
        }

        [Fact]
        public async Task NotificationHasFutureMovement_ReturnsEmpty()
        {
            AddNotification();

            notificationMovement.Date = NewestDate;

            notificationMovement.Receipt = new TestableMovementReceipt
            {
                Date = NewestDate,
                Decision = Decision.Accepted,
                Quantity = 2
            };

            Context.Movements.Add(notificationMovement);

            var result = await handler.HandleAsync(request);

            Assert.Empty(result.MovementDatas);
        }

        [Fact]
        public async Task NotificationHasActiveMovementWithoutReceiptCertificate_ReturnsEmpty()
        {
            AddNotification();
            Context.Movements.Add(notificationMovement);

            var result = await handler.HandleAsync(request);

            Assert.Empty(result.MovementDatas);
        }

        [Fact]
        public async Task NotificationHasActiveMovementWithCertificateOfReceipt_ReturnsResult()
        {
            AddNotification();

            notificationMovement.Receipt = new TestableMovementReceipt
            {
                Date = NewestDate,
                Decision = Decision.Accepted,
                Quantity = 2
            };

            Context.Movements.Add(notificationMovement);

            var result =
                await
                    handler.HandleAsync(request);

            Assert.Single(result.MovementDatas, m => m.Id == MovementId
                && m.Number == TestInt);
        }

        private void AddNotification()
        {
            notificationApplication.Id = NotificationId;
            notificationApplication.NotificationType = NotificationType.Recovery;
            notificationApplication.UserId = UserId;
            Context.NotificationApplications.Add(notificationApplication);
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }
    }
}
