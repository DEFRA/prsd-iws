namespace EA.Iws.Requests.Notification.Overview
{
    using System;
    using WasteRecovery;

    public class WasteDisposalOverview
    {
        public Guid NotificationId { get; set; }
        public string DisposalMethod { get; set; }
        public ValuePerWeightData DisposalCost { get; set; }
    }
}
