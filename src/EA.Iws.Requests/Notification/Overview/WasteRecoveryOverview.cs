namespace EA.Iws.Requests.Notification.Overview
{
    using System;
    using Requests.WasteRecovery;

    public class WasteRecoveryOverview
    {
        public Guid NotificationId { get; set; }
        public decimal PercentageRecoverable { get; set; }
        public ValuePerWeightData EstimatedValue { get; set; }
        public ValuePerWeightData RecoveryCost { get; set; }
    }
}
