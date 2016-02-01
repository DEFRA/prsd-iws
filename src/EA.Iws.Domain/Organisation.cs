namespace EA.Iws.Domain
{
    using Core.Shared;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class Organisation : Entity
    {
        public Organisation(string name, BusinessType type, string otherDescription = null)
        {
            Guard.ArgumentNotNullOrEmpty(() => name, name);
            Guard.ArgumentNotDefaultValue(() => type, type);

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

        public void Update(string name, BusinessType type, string otherDescription = null)
        {
            Guard.ArgumentNotNullOrEmpty(() => name, name);
            Guard.ArgumentNotDefaultValue(() => type, type);
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