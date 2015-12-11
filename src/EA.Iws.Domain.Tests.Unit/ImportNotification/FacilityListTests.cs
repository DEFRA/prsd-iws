namespace EA.Iws.Domain.Tests.Unit.ImportNotification
{
    using System;
    using Core.Shared;
    using Domain.ImportNotification;
    using Xunit;

    internal class FacilityListTests
    {
        private readonly Address address;
        private readonly Contact contact;
        private readonly Guid countryId = new Guid("1B685DF3-C190-4359-A81C-537E7AD5BA68");

        public FacilityListTests()
        {
            address = new Address("line1", "line2", "town", "postcode", countryId);
            contact = new Contact("name", new PhoneNumber("123"), new EmailAddress("a@b.com"));
        }

        [Fact]
        public void MustHaveActualSiteOfTreatment()
        {
            var facility = new Facility("business", BusinessType.LimitedCompany, "123", address, contact, false);
            var facility2 = new Facility("business", BusinessType.LimitedCompany, "123", address, contact, false);

            var facilities = new[]
            {
                facility,
                facility2
            };

            Action createFacilityCollection = () => new FacilityList(facilities);

            Assert.Throws<ArgumentException>("facilities", createFacilityCollection);
        }

        [Fact]
        public void CantHaveMultipleSiteOfTreatment()
        {
            var facility = new Facility("business", BusinessType.LimitedCompany, "123", address, contact, true);
            var facility2 = new Facility("business", BusinessType.LimitedCompany, "123", address, contact, true);

            var facilities = new[]
            {
                facility,
                facility2
            };

            Action createFacilityCollection = () => new FacilityList(facilities);

            Assert.Throws<ArgumentException>("facilities", createFacilityCollection);
        }
    }
}