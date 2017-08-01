namespace EA.Iws.Core.Movement
{
    using Core.Shared;
    using FinancialGuarantee;
    using Notification;
    using NotificationAssessment;
    using System;

    public class InternalMovementSummary
    {
        public Guid NotificationId { get; set; }

        public string NotificationNumber { get; set; }

        public int TotalShipments { get; set; }

        public int ActiveLoadsPermitted { get; set; }

        public int CurrentActiveLoads { get; set; }

        public decimal QuantityReceived { get; set; }

        public decimal QuantityRemaining { get; set; }

        public ShipmentQuantityUnits DisplayUnit { get; set; }

        public FinancialGuaranteeStatus FinancialGuaranteeStatus { get; set; }

        public UKCompetentAuthority CompetentAuthority { get; set; }

        public NotificationStatus NotificationStatus { get; set; }

        public int TotalIntendedShipments { get; set; }

        public decimal AverageTonnage { get; set; }

        public ShipmentQuantityUnits AverageDataUnit { get; set; }
    }
}
