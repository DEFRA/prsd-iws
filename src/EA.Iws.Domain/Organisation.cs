namespace EA.Iws.Domain
{
    using Core.Domain;
    using Utils;

    public class Organisation : Entity
    {
        public string Name { get; private set; }

        public Address Address { get; private set; }

        public string Type { get; private set; }

        public string CompaniesHouseNumber { get; private set; }

        public Organisation(string name, Address address, string type, string companiesHouseNumber = null)
        {
            Guard.ArgumentNotNull(name);
            Guard.ArgumentNotNull(address);
            Guard.ArgumentNotNull(type);

            this.Name = name;
            this.Address = address;
            this.Type = type;
            this.CompaniesHouseNumber = companiesHouseNumber;
        }

        private Organisation()
        {
        }
    }
}
