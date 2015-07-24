namespace EA.Iws.Domain.NotificationApplication
{
    using Prsd.Core;

    public partial class NotificationApplication
    {
        public bool HasImporter
        {
            get { return Importer != null; }
        }

        public void SetImporter(Business business, Address address, Contact contact)
        {
            Guard.ArgumentNotNull(() => business, business);
            Guard.ArgumentNotNull(() => address, address);
            Guard.ArgumentNotNull(() => contact, contact);

            Importer = new Importer(business, address, contact);
        }

        public void RemoveImporter()
        {
            Importer = null;
        }
    }
}