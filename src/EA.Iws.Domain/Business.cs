namespace EA.Iws.Domain
{
    using System;
    using Prsd.Core;

    public class Business
    {
        private string otherDescription;

        public Business(string name, string type, string registrationNumber, string additionalRegistrationNumber)
        {
            Guard.ArgumentNotNull(() => name, name);
            Guard.ArgumentNotNull(() => type, type);

            Name = name;
            Type = type;
            RegistrationNumber = registrationNumber;
            AdditionalRegistrationNumber = additionalRegistrationNumber;
        }

        private Business(string name, BusinessType type, string registrationNumber, string additionalRegistrationNumber, string otherDescription)
        {
            Guard.ArgumentNotNullOrEmpty(() => name, name);
            Guard.ArgumentNotNullOrEmpty(() => registrationNumber, registrationNumber);

            Name = name;
            Type = type.DisplayName;
            RegistrationNumber = registrationNumber;
            AdditionalRegistrationNumber = additionalRegistrationNumber;
            OtherDescription = otherDescription;
        }

        protected Business()
        {
        }

        public string Name { get; private set; }

        public string Type { get; private set; }

        public string RegistrationNumber { get; private set; }

        public string AdditionalRegistrationNumber { get; private set; }

        public string OtherDescription
        {
            get
            {
                return otherDescription;
            }
            private set
            {
                if (Type == BusinessType.Other.DisplayName)
                {
                    Guard.ArgumentNotNullOrEmpty(() => value, value);
                    otherDescription = value;
                }
                else if (!string.IsNullOrEmpty(value))
                {
                    throw new InvalidOperationException("Cannot set other description when business type is not 'other'");
                }
            }
        }

        public static Business CreateBusiness(string name, BusinessType type, string registrationNumber,
            string additionalRegistrationNumber)
        {
            if (type == BusinessType.Other)
            {
                throw new InvalidOperationException("Use CreateOtherBusiness factory method to create a business of type 'Other'");
            }
            return new Business(name, type, registrationNumber, additionalRegistrationNumber, otherDescription: null);
        }

        public static Business CreateOtherBusiness(string name, string registrationNumber,
            string additionalRegistrationNumber, string otherDescription)
        {
            return new Business(name, BusinessType.Other, registrationNumber, additionalRegistrationNumber, otherDescription);
        }
    }
}