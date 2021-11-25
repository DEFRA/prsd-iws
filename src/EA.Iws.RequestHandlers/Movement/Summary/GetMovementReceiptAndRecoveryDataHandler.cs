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
        private readonly IMovementPartialRejectionRepository movementPartialRejectionRepository;

        public GetMovementReceiptAndRecoveryDataHandler(IMovementRepository movementRepository, 
            INotificationApplicationRepository notificationApplicationRepository,
            IMovementRejectionRepository movementRejectionRepository,
            IShipmentInfoRepository shipmentInfoRepository,
            IMovementPartialRejectionRepository movementPartialRejectionRepository)
        {
            this.movementRepository = movementRepository;
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.movementRejectionRepository = movementRejectionRepository;
            this.shipmentInfoRepository = shipmentInfoRepository;
            this.movementPartialRejectionRepository = movementPartialRejectionRepository;
        }

        public async Task<MovementReceiptAndRecoveryData> HandleAsync(GetMovementReceiptAndRecoveryData message)
        {
            var movement = await movementRepository.GetById(message.MovementId);

            var notification = await notificationApplicationRepository.GetById(movement.NotificationId);

            var movementRejection = await movementRejectionRepository.GetByMovementIdOrDefault(movement.Id);

            var movementPartialRejection = await movementPartialRejectionRepository.GetByMovementIdOrDefault(movement.Id);

            var shipmentInfo = await shipmentInfoRepository.GetByNotificationId(notification.Id);

            var data = new MovementReceiptAndRecoveryData
            {
                Id = movement.Id,
                NotificationId = notification.Id,
                Number = movement.Number,
                PossibleUnits = ShipmentQuantityUnitsMetadata.GetUnitsOfThisType(shipmentInfo.Units).ToArray(),
                ActualDate = movement.Date,
                NotificationType = notification.NotificationType,
                PrenotificationDate = movement.PrenotificationDate,
                NotificationUnits = shipmentInfo.Units,
                Status = movement.Status,
                Comments = movement.Comments,
                StatsMarking = movement.StatsMarking
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
                data.ReceiptDate = movementRejection.Date;
                data.RejectedQuantity = movementRejection.RejectedQuantity;
                data.RejectedUnit = movementRejection.RejectedUnit;
                data.IsRejected = true;
            }

            if (movementPartialRejection != null)
            {
                data.RejectionReason = movementPartialRejection.Reason;
                data.ReceiptDate = movementPartialRejection.Date;
                data.ActualQuantity = movementPartialRejection.ActualQuantity;
                data.ReceiptUnits = movementPartialRejection.ActualUnit;
                data.RejectedQuantity = movementPartialRejection.RejectedQuantity;
                data.RejectedUnit = movementPartialRejection.RejectedUnit;
                data.IsPartiallyRejected = true;
            }

            return data;
        }
    }
}
