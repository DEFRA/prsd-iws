namespace EA.Iws.RequestHandlers.Movement.Summary
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Shared;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;
    using Prsd.Core.Mediator;
    using Requests.Movement.Summary;

    internal class GetMovementReceiptAndRecoveryDataHandler : IRequestHandler<GetMovementReceiptAndRecoveryData, MovementReceiptAndRecoveryData>
    {
        private readonly IMovementRepository movementRepository;
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly IMovementRejectionRepository movementRejectionRepository;
        private readonly IShipmentInfoRepository shipmentInfoRepository;

        public GetMovementReceiptAndRecoveryDataHandler(IMovementRepository movementRepository, 
            INotificationApplicationRepository notificationApplicationRepository,
            IMovementRejectionRepository movementRejectionRepository,
            IShipmentInfoRepository shipmentInfoRepository)
        {
            this.movementRepository = movementRepository;
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.movementRejectionRepository = movementRejectionRepository;
            this.shipmentInfoRepository = shipmentInfoRepository;
        }

        public async Task<MovementReceiptAndRecoveryData> HandleAsync(GetMovementReceiptAndRecoveryData message)
        {
            var movement = await movementRepository.GetById(message.MovementId);

            var notification = await notificationApplicationRepository.GetById(movement.NotificationId);

            var movementRejection = await movementRejectionRepository.GetByMovementIdOrDefault(movement.Id);

            var shipmentInfo = await shipmentInfoRepository.GetByNotificationId(notification.Id);

            var data = new MovementReceiptAndRecoveryData
            {
                Id = movement.Id,
                NotificationId = notification.Id,
                Number = movement.Number,
                PossibleUnits = ShipmentQuantityUnitsMetadata.GetUnitsOfThisType(shipmentInfo.Units).ToArray(),
                ActualDate = movement.Date,
                NotificationType = notification.NotificationType,
                PrenotificationDate = movement.PrenotificationDate
            };

            if (movement.Receipt != null)
            {
                data.ReceiptDate = movement.Receipt.Date;
                data.ActualQuantity = movement.Receipt.QuantityReceived.Quantity;
                data.ReceiptUnits = movement.Receipt.QuantityReceived.Units;
                data.IsReceived = true;
            }

            if (movement.CompletedReceipt != null)
            {
                data.OperationCompleteDate = movement.CompletedReceipt.Date;
                data.IsOperationCompleted = true;
            }

            if (movementRejection != null)
            {
                data.RejectionReason = movementRejection.Reason;
                data.RejectionReasonFurtherInformation = movementRejection.FurtherDetails;
                data.ReceiptDate = movementRejection.Date;
            }

            return data;
        }
    }
}
