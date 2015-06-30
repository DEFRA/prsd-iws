namespace EA.Iws.Domain
{
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class Organisation : Entity
    {
        public Organisation(string name, Address address, BusinessType type, string otherDescription = null)
        {
            Guard.ArgumentNotNull(() => name, name);
            Guard.ArgumentNotNull(() => address, address);
            Guard.ArgumentNotNull(() => type, type);

            if (type == BusinessType.Other)
            {
                Guard.ArgumentNotNullOrEmpty(() => otherDescription, otherDescription);
            }

            Name = name;
            Address = address;
            Type = type.DisplayName;
            OtherDescription = otherDescription;
        }

        protected Organisation()
        {
        }

        public string Name { get; private set; }

        public Address Address { get; private set; }

        public string Type { get; private set; }

        public string RegistrationNumber { get; private set; }

        public string OtherDescription { get; private set; }    
    }
}