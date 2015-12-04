namespace EA.Iws.Domain.NotificationApplication
{
    using Prsd.Core;

    public class Contact
    {
        public Contact(string firstName,
            string lastName,
            string telephone,
            string email,
            string fax = null)
        {
            Guard.ArgumentNotNull(() => firstName, firstName);
            Guard.ArgumentNotNull(() => lastName, lastName);
            Guard.ArgumentNotNull(() => telephone, telephone);
            Guard.ArgumentNotNull(() => email, email);

            FirstName = firstName;
            LastName = lastName;
            Telephone = telephone;
            Email = email;
            Fax = fax;
        }

        protected Contact()
        {
        }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Telephone { get; private set; }

        public string Fax { get; private set; }

        public string Email { get; private set; }
    }
}