namespace EA.Iws.Core.Registration.Users
{
    using Organisations;
    using Shared;

    public class User
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public AddressData Address { get; private set; }

        public virtual OrganisationData Organisation { get; set; }
    }
}