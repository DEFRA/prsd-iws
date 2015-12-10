namespace EA.Iws.RequestHandlers.Mappings.ImportMovement
{
    using System;
    using Core.ImportMovement;
    using Core.Shared;
    using Domain.ImportMovement;
    using Prsd.Core.Mapper;
    using ImportMovement = Domain.ImportMovement.ImportMovement;

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
                    PreNotificationDate = source.Movement.PrenotificationDate
                },
                RecoveryData = new ImportMovementRecoveryData
                {
                    IsOperationCompleted = source.CompletedReceipt != null,
                    OperationCompleteDate = (source.CompletedReceipt == null) ? (DateTimeOffset?)null : source.CompletedReceipt.Date
                },
                ReceiptData = GetReceiptData(source)
            };
        }

        private ImportMovementReceiptData GetReceiptData(ImportMovementSummary source)
        {
            var possibleUnits = new[]
            {
                ShipmentQuantityUnits.CubicMetres,
                ShipmentQuantityUnits.Kilograms
            };

            if (source.Rejection != null)
            {
                return new ImportMovementReceiptData
                {
                    PossibleUnits = possibleUnits,
                    RejectionReason = source.Rejection.Reason,
                    RejectionReasonFurtherInformation = source.Rejection.FurtherDetails,
                    ReceiptDate = source.Rejection.Date
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
                PossibleUnits = possibleUnits
            };
        }
    }
}
