namespace EA.Iws.Core.Notification.Overview
{
    using System;
    using CustomsOffice;
    using StateOfExport;

    public class Journey
    {
        public Guid NotificationId { get; set; }
        public StateOfExportWithTransportRouteData TransportRoute { get; set; }
        public EntryCustomsOfficeAddData EntryCustomsOffice { get; set; }
        public ExitCustomsOfficeAddData ExitCustomsOffice { get; set; }
    }
}
