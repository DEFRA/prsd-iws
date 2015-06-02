namespace EA.Iws.Domain.Notification
{
    using System;

    public partial class NotificationApplication
    {
        public bool HasImporter
        {
            get { return Importer != null; }
        }

        public void AddImporter(Business business, Address address, Contact contact)
        {
            if (Importer != null)
            {
                throw new InvalidOperationException(string.Format("Importer already exists, can't add another to this notification: {0}", Id));
            }

            Importer = new Importer(business, address, contact);
        }

        public void RemoveImporter()
        {
            Importer = null;
        }
    }
}