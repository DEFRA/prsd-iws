namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using Core.Carriers;
    using Core.MeansOfTransport;
    using Core.Notification;
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

        public TransportationViewModel(Transportation transportationInfo, NotificationApplicationCompletionProgress progress)
        {
            NotificationId = transportationInfo.NotificationId;
            NotificationType = transportationInfo.NotificationType;
            IsCarrierCompleted = progress.HasCarrier;
            IsMeansOfTransportCompleted = progress.HasMeansOfTransport;
            IsPackagingTypesCompleted = progress.HasPackagingInfo;
            IsSpecialHandlingCompleted = progress.HasSpecialHandlingRequirements;
            Carriers = transportationInfo.Carriers;
            MeanOfTransport = transportationInfo.MeanOfTransport;
            PackagingData = transportationInfo.PackagingData;
            SpecialHandlingDetails = transportationInfo.SpecialHandlingDetails;
        }
    }
}