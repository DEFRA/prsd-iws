namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using Prsd.Core;

    public class ProducerBusiness : Business
    {
        private const string RegistrationNumberWhenEmpty = "Not applicable";

        protected ProducerBusiness()
        {
        }

        private ProducerBusiness(string name, BusinessType type, string registrationNumber,
            string additionalRegistrationNumber, string otherDescription)
            : base(name, type, registrationNumber, additionalRegistrationNumber, otherDescription)
        {
        }

        public static ProducerBusiness CreateProducerBusiness(string name, BusinessType type, string registrationNumber,
            string otherDescription)
        {
            if (type == BusinessType.Other)
            {
                Guard.ArgumentNotNullOrEmpty(() => otherDescription, otherDescription);
            }

            return new ProducerBusiness(name, type, GetRegistrationNumber(type, registrationNumber), null, otherDescription);
        }

        private static string GetRegistrationNumber(BusinessType type, string registrationNumber)
        {
            if (type == BusinessType.LimitedCompany)
            {
                if (string.IsNullOrWhiteSpace(registrationNumber))
                {
                    throw new InvalidOperationException("Tried to create a producer address for a limited company with an empty string.");
                }
                return registrationNumber;
            }

            return RegistrationNumberWhenEmpty;
        }
    }
}
