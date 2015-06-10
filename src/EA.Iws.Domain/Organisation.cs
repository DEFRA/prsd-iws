namespace EA.Iws.Domain
{
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class Organisation : Entity
    {
        public Organisation(string name, Address address, string type, string registrationNumber = null)
        {
            Guard.ArgumentNotNull(() => name, name);
            Guard.ArgumentNotNull(() => address, address);
            Guard.ArgumentNotNull(() => type, type);

            Name = name;
            Address = address;
            Type = type;
            RegistrationNumber = registrationNumber;
        }

        protected Organisation()
        {
        }

        public string Name { get; private set; }

        public Address Address { get; private set; }

        public string Type { get; private set; }

        public string RegistrationNumber { get; private set; }
    }
}