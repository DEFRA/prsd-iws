namespace EA.Iws.RequestHandlers.Tests.Unit.MovementOperationReceipt
{
    using System;
    using System.Threading.Tasks;
    using Core.Shared;
    using RequestHandlers.MovementOperationReceipt;
    using Requests.MovementOperationReceipt;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class GetMovementOperationReceiptDataByMovementIdHandlerTests : TestBase
    {
        private readonly GetMovementOperationReceiptDataByMovementIdHandler handler;
        private readonly GetMovementOperationReceiptDataByMovementId request;

        private static readonly DateTime AnyDate = new DateTime(2015, 9, 1);

        public GetMovementOperationReceiptDataByMovementIdHandlerTests()
        {
            NotificationApplication.NotificationType = NotificationType.Recovery;
            Context.NotificationApplications.Add(NotificationApplication);
            Context.Movements.Add(Movement);
            handler = new GetMovementOperationReceiptDataByMovementIdHandler(Context);
            request = new GetMovementOperationReceiptDataByMovementId(MovementId);
        }

        [Fact]
        public async Task MovementNotExists_Throws()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.HandleAsync(new GetMovementOperationReceiptDataByMovementId(Guid.Empty)));
        }

        [Theory]
        [InlineData(NotificationType.Recovery)]
        [InlineData(NotificationType.Disposal)]
        public async Task ReturnsCorrectNotificationType(NotificationType notificationType)
        {
            if (notificationType == NotificationType.Recovery)
            {
                NotificationApplication.NotificationType = NotificationType.Recovery;
            }
            else
            {
                NotificationApplication.NotificationType = NotificationType.Disposal;
            }

            var result = await handler.HandleAsync(request);

            Assert.Equal(notificationType, result.NotificationType);
        }

        [Fact]
        public async Task ReturnsNullDateWhenNoOperationReceipt()
        {
            var result = await handler.HandleAsync(request);

            Assert.Null(result.DateCompleted);
        }

        [Fact]
        public async Task ReturnsDateWhenOperationReceiptExists()
        {
            Movement.Receipt = new TestableMovementReceipt
            {
                OperationReceipt = new TestableMovementOperationReceipt { Date = AnyDate }
            };

            var result = await handler.HandleAsync(request);

            Assert.Equal(AnyDate, result.DateCompleted);
        }
    }
}
