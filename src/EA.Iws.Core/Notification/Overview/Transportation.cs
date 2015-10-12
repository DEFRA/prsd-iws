namespace EA.Iws.Core.Notification.Overview
{
    using System;
    using System.Collections.Generic;
    using Carriers;
    using MeansOfTransport;
    using Shared;

    public class Transportation
    {
        public Guid NotificationId { get; set; }
        public NotificationType NotificationType { get; set; }
        public List<CarrierData> Carriers { get; set; }
        public List<MeansOfTransport> MeanOfTransport { get; set; }
        public List<string> PackagingData { get; set; }
        public string SpecialHandlingDetails { get; set; }
    }
}
