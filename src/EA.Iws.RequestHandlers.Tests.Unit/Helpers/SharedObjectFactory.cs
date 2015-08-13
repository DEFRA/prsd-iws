namespace EA.Iws.RequestHandlers.Tests.Unit.Helpers
{
    using System;
    using Core.Shared;

    public static class SharedObjectFactory
    {
        public static AddressData GetAddressData(Guid countryId)
        {
            return new AddressData()
            {
                Address2 = "Address2",
                CountryId = countryId,
                CountryName = "test",
                DefaultCountryId = countryId,
                PostalCode = "GU227UY",
                Region = "Surrey",
                StreetOrSuburb = "Street Or Suburb",
                TownOrCity = "Home Town"
            };
        }

        public static BusinessInfoData GetBusinessInfoData()
        {
            return new BusinessInfoData()
            {
                AdditionalRegistrationNumber = "REGNUMBER0001",
                BusinessType = BusinessType.SoleTrader,
                Name = "My exporter company",
                RegistrationNumber = "REGNO 0001"
            };
        }

        public static ContactData GetContactData()
        {
            return new ContactData()
            {
                Email = "atestemailaddress@atestdomain.com",
                Fax = "1234567890",
                FirstName = "John",
                LastName = "Botham",
                Telephone = "2233445566"
            };
        }
    }
}
