namespace EA.Iws.Core.Movement
{
    using System;
    using System.Collections.Generic;
    using Shared;

    public class MovementReceiptAndRecoveryData
    {
        public Guid NotificationId { get; set; }

        public NotificationType NotificationType { get; set; }

        public Guid Id { get; set; }
        
        public int Number { get; set; }

        public DateTime ActualDate { get; set; }

        public DateTime? PrenotificationDate { get; set; }

        public DateTime? ReceiptDate { get; set; }

        public decimal? ActualQuantity { get; set; }

        public ShipmentQuantityUnits NotificationUnits { get; set; }

        public ShipmentQuantityUnits? ReceiptUnits { get; set; }

        public IList<ShipmentQuantityUnits> PossibleUnits { get; set; }

        public string RejectionReason { get; set; }

        public DateTime? OperationCompleteDate { get; set; }

        public bool IsReceived { get; set; }

        public bool IsOperationCompleted { get; set; }

        public bool IsRejected { get; set; }

        public MovementStatus Status { get; set; }

        public string Comments { get; set; }

        public string StatsMarking { get; set; }

        public bool HasNoPrenotification { get; set; }

        public DateTime? RejectionDate { get; set; }
    }
}
