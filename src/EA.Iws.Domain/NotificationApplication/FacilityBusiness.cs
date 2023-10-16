namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using Core.Shared;
    using Prsd.Core;

    public class FacilityBusiness : Business
    {
        private const string RegistrationNumberWhenEmpty = "Not applicable";

        protected FacilityBusiness()
        {
        }

        private FacilityBusiness(string name, BusinessType type, string registrationNumber, string additionalRegistrationNumber, string otherDescription)
            : base(name, type, registrationNumber, additionalRegistrationNumber, otherDescription)
        {
        }

        public static FacilityBusiness CreateFacilityBusiness(string name, BusinessType type, string registrationNumber,
            string otherDescription)
        {
            if (type == BusinessType.Other)
            {
                Guard.ArgumentNotNullOrEmpty(() => otherDescription, otherDescription);
            }

            return new FacilityBusiness(name, type, GetRegistrationNumber(type, registrationNumber), null, otherDescription);
        }

        private static string GetRegistrationNumber(BusinessType type, string registrationNumber)
        {
            if (type == BusinessType.LimitedCompany)
            {
                if (string.IsNullOrWhiteSpace(registrationNumber))
                {
                    throw new InvalidOperationException("Tried to create a facility address for a limited company with an empty string.");
                }
                return registrationNumber;
            }

            return RegistrationNumberWhenEmpty;
        }
    }
}
