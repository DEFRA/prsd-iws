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
                throw new InvalidOperationException(string.Format("Exporter already exists, can't add another to this notification: {0}", Id));
            }

            Exporter = new Exporter(business, address, contact);
        }

        public void RemoveExporter()
        {
            Exporter = null;
        }
    }
}