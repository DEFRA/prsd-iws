namespace EA.Iws.Domain.NotificationApplication
{
    using Prsd.Core;

    public class Contact
    {
        public Contact(string fullName,
            string telephone,
            string email,
            string fax = null)
        {
            Guard.ArgumentNotNull(() => fullName, fullName);
            Guard.ArgumentNotNull(() => telephone, telephone);
            Guard.ArgumentNotNull(() => email, email);

            FullName = fullName;
            Telephone = telephone;
            Email = email;
            Fax = fax;
        }

        protected Contact()
        {
        }

        public string FullName { get; private set; }

        public string Telephone { get; private set; }

        public string Fax { get; private set; }

        public string Email { get; private set; }
    }
}