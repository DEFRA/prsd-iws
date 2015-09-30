namespace EA.Iws.Requests.Notification.Overview
{
    using System;
    using CustomsOffice;
    using StateOfExport;

    public class Journey
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
