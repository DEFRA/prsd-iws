namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using System;
    using Core.ImportNotification.Draft;

    internal class AddressTestData
    {
        public static Address GetValidTestAddress()
        {
            return new Address
            {
                AddressLine1 = "Eliot House",
                AddressLine2 = "Eliot Lane",
                CountryId = new Guid("0A323947-78A0-40FE-BDDD-9A58803559BC"),
                PostalCode = "EL10TJ",
                TownOrCity = "Eliotsville"
            };
        }

        public static Address GetValidTestAddress(Guid countryId)
        {
            return new Address
            {
                AddressLine1 = "Eliot House",
                AddressLine2 = "Eliot Lane",
                CountryId = countryId,
                PostalCode = "EL10TJ",
                TownOrCity = "Eliotsville"
            };
        }
    }
}