namespace EA.Iws.Domain
{
    using System;
    using NotificationApplication;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class Organisation : Entity
    {
        public Organisation(string name, BusinessType type, string otherDescription = null)
        {
            Guard.ArgumentNotNullOrEmpty(() => name, name);
            Guard.ArgumentNotNull(() => type, type);

            if (type == BusinessType.Other)
            {
                Guard.ArgumentNotNullOrEmpty(() => otherDescription, otherDescription);
            }

            Name = name;
            Type = type;
            OtherDescription = otherDescription;
        }

        protected Organisation()
        {
        }

        public string Name { get; private set; }

        public BusinessType Type { get; private set; }

        public string RegistrationNumber { get; private set; }

        public string OtherDescription { get; private set; }

        public void Update(string name, Address address, BusinessType type, string otherDescription = null)
        {
            Guard.ArgumentNotNullOrEmpty(() => name, name);
            Guard.ArgumentNotNull(() => address, address);
            Guard.ArgumentNotNull(() => type, type);
            if (type == BusinessType.Other)
            {
                Guard.ArgumentNotNullOrEmpty(() => otherDescription, otherDescription);
            }

            Name = name;
            Type = type;
            OtherDescription = otherDescription;
        }
    }
}