namespace EA.Iws.Domain
{
    using Core.Domain;
    using Utils;

    internal class User : Entity
    {
        public string FirstName { get; private set; }

        public string Surname { get; private set; }

        public string PhoneNumber { get; private set; }

        public string Email { get; private set; }

        public virtual Organisation Organisation { get; private set; }

        public User(string firstName, string surname, string phoneNumber, string email)
        {
            Guard.ArgumentNotNull(firstName);
            Guard.ArgumentNotNull(surname);
            Guard.ArgumentNotNull(phoneNumber);
            Guard.ArgumentNotNull(email);

            FirstName = firstName;
            Surname = surname;
            PhoneNumber = phoneNumber;
            Email = email;
        }
    }
}