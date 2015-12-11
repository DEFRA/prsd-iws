namespace EA.Iws.Domain.Tests.Unit.ImportNotification
{
    using System;
    using System.Collections.Generic;
    using Core.Shared;
    using Domain.ImportNotification;
    using Xunit;

    public class FacilityCollectionTests
    {
        private readonly Guid importNotificationId = new Guid("1FE8238D-BD7A-4D3A-8188-704EFB2F62F4");
        private readonly Guid countryId = new Guid("1B685DF3-C190-4359-A81C-537E7AD5BA68");

        private readonly FacilityList validFacilities;
        private readonly Address address;
        private readonly Contact contact;

        public FacilityCollectionTests()
        {
            address = new Address("line1", "line2", "town", "postcode", countryId);
            contact = new Contact("name", new PhoneNumber("123"), new EmailAddress("a@b.com"));
            var facility = new Facility("business", BusinessType.LimitedCompany, "123", address, contact, true);
            var facility2 = new Facility("business", BusinessType.LimitedCompany, "123", address, contact, false);

            validFacilities = new FacilityList(new[]
            {
                facility,
                facility2
            });
        }

        [Fact]
        public void CanCreateFacilityCollection()
        {
            var facilityCollection = new FacilityCollection(importNotificationId, validFacilities, true);

            Assert.IsType<FacilityCollection>(facilityCollection);
        }
    }
}