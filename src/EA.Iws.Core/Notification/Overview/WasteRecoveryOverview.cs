namespace EA.Iws.Core.Notification.Overview
{
    using System;
    using Shared;

    public class WasteRecoveryOverview
    {
        public Guid NotificationId { get; set; }
        public decimal PercentageRecoverable { get; set; }
        public ValuePerWeightData EstimatedValue { get; set; }
        public ValuePerWeightData RecoveryCost { get; set; }
    }
}
