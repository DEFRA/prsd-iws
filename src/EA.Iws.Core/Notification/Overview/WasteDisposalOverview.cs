namespace EA.Iws.Core.Notification.Overview
{
    using System;
    using Shared;

    public class WasteDisposalOverview
    {
        public Guid NotificationId { get; set; }
        public string DisposalMethod { get; set; }
        public ValuePerWeightData DisposalCost { get; set; }
    }
}
