namespace EA.Iws.Domain.Notification
{
    using System;

    public partial class NotificationApplication
    {
        public bool HasExporter
        {
            get { return Exporter != null; }
        }

        public void AddExporter(Business business, Address address, Contact contact)
        {
            if (Exporter != null)
            {
                throw new InvalidOperationException("Exporter already exists, can't add another.");
            }

            Exporter = new Exporter(business, address, contact);
        }

        public void RemoveExporter()
        {
            Exporter = null;
        }
    }
}