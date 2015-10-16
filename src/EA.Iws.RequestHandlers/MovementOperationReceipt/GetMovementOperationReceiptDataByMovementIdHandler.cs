namespace EA.Iws.RequestHandlers.MovementOperationReceipt
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.MovementOperationReceipt;

    internal class GetMovementOperationReceiptDataByMovementIdHandler : IRequestHandler<GetMovementOperationReceiptDataByMovementId, MovementOperationReceiptData>
    {
        private readonly IwsContext context;

        public GetMovementOperationReceiptDataByMovementIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<MovementOperationReceiptData> HandleAsync(GetMovementOperationReceiptDataByMovementId message)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == message.Id);

            var notification = await context.GetNotificationApplication(movement.NotificationId);

            DateTime? dateCompleted = null;

            if (movement.Receipt != null && movement.Receipt.OperationReceipt != null)
            {
                dateCompleted = movement.Receipt.OperationReceipt.Date;
            }

            return new MovementOperationReceiptData
            {
                NotificationType = (NotificationType)notification.NotificationType,
                DateCompleted = dateCompleted
            };
        }
    }
}
