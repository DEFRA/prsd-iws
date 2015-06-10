namespace EA.Iws.Domain
{
    using System;
    using Prsd.Core;

    public class User
    {
        protected User()
        {
        }

        public User(string id, string firstName, string surname, string phoneNumber, string email)
        {
            Guard.ArgumentNotNull(() => firstName, firstName);
            Guard.ArgumentNotNull(() => surname, surname);
            Guard.ArgumentNotNull(() => phoneNumber, phoneNumber);
            Guard.ArgumentNotNull(() => email, email);
            Guard.ArgumentNotNull(() => id, id);

            FirstName = firstName;
            Surname = surname;
            PhoneNumber = phoneNumber;
            Email = email;
            Id = id;
        }

        public string Id { get; private set; }

        public string FirstName { get; private set; }

        public string Surname { get; private set; }

        public string PhoneNumber { get; private set; }

        public string Email { get; private set; }

        public virtual Organisation Organisation { get; private set; }

        public void LinkToOrganisation(Organisation organisation)
        {
            Guard.ArgumentNotNull(() => organisation, organisation);

            if (Organisation != null)
            {
                throw new InvalidOperationException(
                    string.Format("User {0} is already linked to an organisation and may not be linked to another. This user is linked to organisation: {1}", Id, Organisation.Id));
            }

            Organisation = organisation;
        }
    }
}