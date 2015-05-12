namespace EA.Iws.Domain
{
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class Contact : Entity
    {
        public Contact(string firstName,
            string lastName,
            string telephone,
            string email,
            string fax = null)
        {
            Guard.ArgumentNotNull(firstName);
            Guard.ArgumentNotNull(lastName);
            Guard.ArgumentNotNull(telephone);
            Guard.ArgumentNotNull(email);

            FirstName = firstName;
            LastName = lastName;
            Telephone = telephone;
            Email = email;
            Fax = fax;
        }

        private Contact()
        {
        }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Telephone { get; private set; }

        public string Fax { get; private set; }

        public string Email { get; private set; }
    }
}