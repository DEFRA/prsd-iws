namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using Core.Carriers;
    using Core.MeansOfTransport;
    using Core.Shared;
    using Requests.Notification.Overview;

    public class TransportationViewModel
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

        public TransportationViewModel()
        {
        }

        public TransportationViewModel(Transportation transportationInfo)
        {
            NotificationId = transportationInfo.NotificationId;
            NotificationType = transportationInfo.NotificationType;
            IsCarrierCompleted = transportationInfo.IsCarrierCompleted;
            IsMeansOfTransportCompleted = transportationInfo.IsMeansOfTransportCompleted;
            IsPackagingTypesCompleted = transportationInfo.IsPackagingTypesCompleted;
            IsSpecialHandlingCompleted = transportationInfo.IsSpecialHandlingCompleted;
            Carriers = transportationInfo.Carriers;
            MeanOfTransport = transportationInfo.MeanOfTransport;
            PackagingData = transportationInfo.PackagingData;
            SpecialHandlingDetails = transportationInfo.SpecialHandlingDetails;
        }
    }
}