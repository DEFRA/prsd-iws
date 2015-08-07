namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Notification;
    using Requests.CustomsOffice;
    using Requests.StateOfExport;

    public class JourneyInfo
    {
        public Guid NotificationId { get; set; }
        public bool IsStateOfExportCompleted { get; set; }
        public bool IsStateOfImportCompleted { get; set; }
        public bool AreTransitStatesCompleted { get; set; }
        public bool IsCustomsOfficeCompleted { get; set; }
        public StateOfExportWithTransportRouteData TransportRoute { get; set; }
        public EntryCustomsOfficeAddData EntryCustomsOffice { get; set; }
        public ExitCustomsOfficeAddData ExitCustomsOffice { get; set; }
    }
}
