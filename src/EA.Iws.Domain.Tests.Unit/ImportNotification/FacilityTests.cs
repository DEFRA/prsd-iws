namespace EA.Iws.Domain.Tests.Unit.ImportNotification
{
    using System;
    using Core.Shared;
    using Domain.ImportNotification;
    using Xunit;

    public class FacilityTests
    {
        private readonly Guid importNotificationId = new Guid("1FE8238D-BD7A-4D3A-8188-704EFB2F62F4");
        private readonly Guid countryId = new Guid("1B685DF3-C190-4359-A81C-537E7AD5BA68");
        private readonly Address address;
        private readonly Contact contact;

        public FacilityTests()
        {
            address = new Address("line1", "line2", "town", "postcode", countryId);
            contact = new Contact("name", new PhoneNumber("123"), new EmailAddress("a@b.com"));
        }

        [Fact]
        public void CanCreateFacility()
        {
            var importer = new Facility("business name", BusinessType.LimitedCompany, "reg no", address, contact, true);

            Assert.IsType<Facility>(importer);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        public void BusinessNameCantBeNullOrEmpty(string input, Type expectedException)
        {
            Action createFacility = () => new Facility(input, BusinessType.LimitedCompany, "reg no", address, contact, true);

            Assert.Throws(expectedException, createFacility);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void RegistrationNumberCanBeNullOrEmpty(string input)
        {
            var facility = new Facility("business name", BusinessType.LimitedCompany, input, address, contact, true);

            Assert.IsType<Facility>(facility);
        }

        [Fact]
        public void BusinessTypeCantBeDefault()
        {
            Action createFacility = () => new Facility("business name", default(BusinessType), "reg no", address, contact, true);

            Assert.Throws<ArgumentException>("businessType", createFacility);
        }

        [Fact]
        public void AddressCantBeNull()
        {
            Action createFacility = () => new Facility("business name", BusinessType.LimitedCompany, "reg no", null, contact, true);

            Assert.Throws<ArgumentNullException>("address", createFacility);
        }

        [Fact]
        public void ContactCantBeNull()
        {
            Action createFacility = () => new Facility("business name", BusinessType.LimitedCompany, "reg no", address, null, true);

            Assert.Throws<ArgumentNullException>("contact", createFacility);
        }
    }
}
