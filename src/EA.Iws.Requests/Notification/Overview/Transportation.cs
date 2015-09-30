namespace EA.Iws.Requests.Notification.Overview
{
    using System;
    using System.Collections.Generic;
    using Core.Carriers;
    using Core.MeansOfTransport;
    using Core.Shared;

    public class Transportation
    {
        public Guid NotificationId { get; set; }
        public NotificationType NotificationType { get; set; }
        public bool IsCarrierCompleted { get; set; }
        public bool IsMeansOfTransportCompleted { get; set; }
        public bool IsPackagingTypesCompleted { get; set; }
        public bool IsSpecialHandlingCompleted { get; set; }
        public List<CarrierData> Carriers { get; set; }
        public List<MeansOfTransport> MeanOfTransport { get; set; }
        public List<string> PackagingData { get; set; }
        public string SpecialHandlingDetails { get; set; }
    }
}
