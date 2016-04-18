namespace EA.Iws.Domain.NotificationApplication
{
    using System;

    public class ExportNotificationOwnerDisplay
    {
        public Guid NotificationId { get; private set; }

        public string Number { get; private set; }

        public string ExporterName { get; private set; }

        public string ImporterName { get; private set; }

        public string ProducerName { get; private set; }

        public string OwnerName { get; private set; }
    }
}