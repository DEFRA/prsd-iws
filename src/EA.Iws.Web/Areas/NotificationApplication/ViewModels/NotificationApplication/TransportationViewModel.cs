namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Core.Carriers;
    using Core.MeansOfTransport;
    using Core.Notification;
    using Core.Shared;
    using Requests.Notification;

    public class TransportationViewModel
    {
        public Guid NotificationId { get; set; }
        public NotificationType NotificationType { get; set; }
        public NotificationApplicationCompletionProgress Progress { get; set; }
        public List<CarrierData> Carriers { get; set; }
        public List<MeansOfTransport> MeanOfTransport { get; set; }
        public List<string> PackagingData { get; set; }
        public string SpecialHandlingDetails { get; set; }

        public TransportationViewModel()
        {
        }

        public TransportationViewModel(TransportationInfo transportationInfo)
        {
            NotificationId = transportationInfo.NotificationId;
            NotificationType = transportationInfo.NotificationType;
            Progress = transportationInfo.Progress;
            Carriers = transportationInfo.Carriers;
            MeanOfTransport = transportationInfo.MeanOfTransport;
            PackagingData = transportationInfo.PackagingData;
            SpecialHandlingDetails = transportationInfo.SpecialHandlingDetails;
        }
    }
}