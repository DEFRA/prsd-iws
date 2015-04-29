namespace EA.Iws.Domain
{
    using System;
    using Utils;

    public class User
    {
        public string Id { get; private set; }

        public string FirstName { get; private set; }

        public string Surname { get; private set; }

        public string PhoneNumber { get; private set; }

        public string Email { get; private set; }

        public virtual Organisation Organisation { get; private set; }

        private User()
        {
        }

        public User(string id, string firstName, string surname, string phoneNumber, string email)
        {
            Guard.ArgumentNotNull(firstName);
            Guard.ArgumentNotNull(surname);
            Guard.ArgumentNotNull(phoneNumber);
            Guard.ArgumentNotNull(email);
            Guard.ArgumentNotNull(id);

            FirstName = firstName;
            Surname = surname;
            PhoneNumber = phoneNumber;
            Email = email;
            Id = id;
        }

        public void LinkToOrganisation(Organisation organisation)
        {
            Guard.ArgumentNotNull(organisation);

            if (this.Organisation != null)
            {
                throw new InvalidOperationException("User is already linked to an organisation and may not be linked to another. This user is linked to organisation: " + this.Organisation.Id);
            }

            this.Organisation = organisation;
        }
    }
}