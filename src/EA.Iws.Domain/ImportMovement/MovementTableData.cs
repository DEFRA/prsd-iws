﻿namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using Core.Shared;

    public class MovementTableData
    {
        public Guid Id { get; private set; }

        public int Number { get; private set; }

        public DateTime? PreNotification { get; private set; }

        public DateTime? ShipmentDate { get; private set; }

        public DateTime? Received { get; private set; }

        public decimal? Quantity { get; private set; }

        public ShipmentQuantityUnits? Unit { get; private set; }

        public DateTime? Rejected { get; private set; }

        public DateTime? RecoveredOrDisposedOf { get; private set; }

        public bool IsCancelled { get; private set; }

        public bool IsPartiallyRejected { get; set; }

        public static MovementTableData Load(ImportMovement movement,
            ImportMovementReceipt movementReceipt,
            ImportMovementRejection movementRejection,
            ImportMovementCompletedReceipt movementOperationReceipt,
            ImportMovementPartialRejection movementPartialRejection)
        {
            var data = new MovementTableData();

            if (movement != null)
            {
                data.Id = movement.Id;
                data.Number = movement.Number;
                data.PreNotification = movement.PrenotificationDate;
                data.ShipmentDate = movement.ActualShipmentDate;
                data.IsCancelled = movement.IsCancelled;
            }

            if (movementReceipt != null)
            {
                data.Received = movementReceipt.Date;
                data.Quantity = movementReceipt.Quantity;
                data.Unit = movementReceipt.Unit;
            }

            if (movementPartialRejection != null)
            {
                data.Received = movementPartialRejection.WasteReceivedDate;
                data.Quantity = movementPartialRejection.ActualQuantity;
                data.Unit = movementPartialRejection.ActualUnit;
                data.IsPartiallyRejected = true;
            }

            if (movementRejection != null)
            {
                data.Rejected = movementRejection.Date;
            }

            if (movementOperationReceipt != null)
            {
                data.RecoveredOrDisposedOf = movementOperationReceipt.Date;
            }

            return data;
        }
    }
}
