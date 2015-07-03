namespace EA.Iws.Requests.Copy
{
    using System;

    public class NotificationApplicationCopyData
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        public string ExporterName { get; set; }

        public string ImporterName { get; set; }

        public string WasteName { get; set; }
    }
}
