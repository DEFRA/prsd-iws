namespace EA.Iws.RequestHandlers.Mappings.ImportMovement
{
    using System;
    using System.Linq;
    using Core.ImportMovement;
    using Core.Shared;
    using Domain.ImportMovement;
    using Prsd.Core.Mapper;

    internal class MovementSummaryMap : IMap<ImportMovementSummary, ImportMovementSummaryData>
    {
        public ImportMovementSummaryData Map(ImportMovementSummary source)
        {
            return new ImportMovementSummaryData
            {
                Data = new ImportMovementData
                {
                    NotificationId = source.Movement.NotificationId,
                    Number = source.Movement.Number,
                    NotificationType = source.NotificationType,
                    ActualDate = source.Movement.ActualShipmentDate,
                    PreNotificationDate = source.Movement.PrenotificationDate,
                    IsCancelled = source.Movement.IsCancelled
                },
                RecoveryData = new ImportMovementRecoveryData
                {
                    IsOperationCompleted = source.CompletedReceipt != null,
                    OperationCompleteDate = (source.CompletedReceipt == null) ? (DateTimeOffset?)null : source.CompletedReceipt.Date
                },
                ReceiptData = GetReceiptData(source),
                Comments = source.Movement.Comments,
                StatsMarking = source.Movement.StatsMarking
            };
        }

        private static ImportMovementReceiptData GetReceiptData(ImportMovementSummary source)
        {
            var possibleUnits = ShipmentQuantityUnitsMetadata.GetUnitsOfThisType(source.Units).ToArray();

            if (source.Rejection != null)
            {
                return new ImportMovementReceiptData
                {
                    PossibleUnits = possibleUnits,
                    RejectionReason = source.Rejection.Reason,
                    ReceiptDate = source.Rejection.Date,
                    IsRejected = true
                };
            }

            if (source.Receipt != null)
            {
                return new ImportMovementReceiptData
                {
                    PossibleUnits = possibleUnits,
                    ReceiptDate = source.Receipt.Date,
                    ActualQuantity = source.Receipt.Quantity,
                    IsReceived = true,
                    ReceiptUnits = source.Receipt.Unit
                };
            }

            return new ImportMovementReceiptData
            {
                IsReceived = false,
                PossibleUnits = possibleUnits,
                NotificationUnit = source.Units
            };
        }
    }
}
