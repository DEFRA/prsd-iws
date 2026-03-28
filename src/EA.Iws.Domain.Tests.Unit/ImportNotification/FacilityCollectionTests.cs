namespace EA.Iws.Domain.Tests.Unit.ImportNotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
            var facility2 = new Facility("business2", BusinessType.LimitedCompany, "456", address, contact, false);

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

        [Fact]
        public void FacilityCollection_AssignsOrdinalPositions()
        {
            var facilityCollection = new FacilityCollection(importNotificationId, validFacilities, true);

            var facilities = facilityCollection.Facilities.ToList();

            Assert.Equal(1, facilities[0].OrdinalPosition);
            Assert.Equal(2, facilities[1].OrdinalPosition);
        }

        [Fact]
        public void FacilityCollection_FacilitiesReturnedInOrdinalOrder()
        {
            var facilityCollection = new FacilityCollection(importNotificationId, validFacilities, true);

            var positions = facilityCollection.Facilities.Select(f => f.OrdinalPosition).ToList();

            Assert.True(positions.SequenceEqual(positions.OrderBy(p => p)));
        }

        [Fact]
        public void FacilityCollection_ThreeFacilities_AssignsSequentialOrdinalPositions()
        {
            var facility3 = new Facility("business3", BusinessType.Other, "789", address, contact, false);

            var threeFacilities = new FacilityList(new[]
            {
                new Facility("business", BusinessType.LimitedCompany, "123", address, contact, true),
                new Facility("business2", BusinessType.LimitedCompany, "456", address, contact, false),
                facility3
            });

            var facilityCollection = new FacilityCollection(importNotificationId, threeFacilities, true);

            var facilities = facilityCollection.Facilities.ToList();

            Assert.Equal(1, facilities[0].OrdinalPosition);
            Assert.Equal(2, facilities[1].OrdinalPosition);
            Assert.Equal(3, facilities[2].OrdinalPosition);
        }
    }
}