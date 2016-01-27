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

        public static ExportNotificationOwnerDisplay Load(Guid notificationId,
            string number,
            Exporter.Exporter exporter,
            Importer.Importer importer,
            Producer producer,
            string ownerName)
        {
            var exporterName = exporter != null ? exporter.Business.Name : string.Empty;
            var importerName = importer != null ? importer.Business.Name : string.Empty;
            var producerName = producer != null ? producer.Business.Name : string.Empty;

            return new ExportNotificationOwnerDisplay
            {
                NotificationId = notificationId,
                Number = number,
                ExporterName = exporterName,
                ImporterName = importerName,
                ProducerName = producerName,
                OwnerName = ownerName
            };
        }
    }
}