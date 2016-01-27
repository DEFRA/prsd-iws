namespace EA.Iws.Core.Admin
{
    using System;

    public class ExportNotificationOwnerDisplay
    {
        public Guid NotificationId { get; set; }

        public string Number { get; set; }

        public string ExporterName { get; set; }

        public string ProducerName { get; set; }

        public string ImporterName { get; set; }

        public string OwnerName { get; set; }
    }
}