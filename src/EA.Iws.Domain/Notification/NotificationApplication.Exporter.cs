namespace EA.Iws.Domain.Notification
{
    using Prsd.Core;

    public partial class NotificationApplication
    {
        public bool HasExporter
        {
            get { return Exporter != null; }
        }

        public void SetExporter(Business business, Address address, Contact contact)
        {
            Guard.ArgumentNotNull(() => business, business);
            Guard.ArgumentNotNull(() => address, address);
            Guard.ArgumentNotNull(() => contact, contact);

            Exporter = new Exporter(business, address, contact);
        }

        public void RemoveExporter()
        {
            Exporter = null;
        }
    }
}