namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Notification;
    using Requests.CustomsOffice;
    using Requests.StateOfExport;

    public class JourneyInfo
    {
        public Guid NotificationId { get; set; }
        public NotificationApplicationCompletionProgress Progress { get; set; }
        public StateOfExportWithTransportRouteData TransportRoute { get; set; }
        public EntryCustomsOfficeAddData EntryCustomsOffice { get; set; }
        public ExitCustomsOfficeAddData ExitCustomsOffice { get; set; }
    }
}
