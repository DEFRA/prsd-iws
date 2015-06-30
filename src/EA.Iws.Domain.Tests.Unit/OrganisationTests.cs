namespace EA.Iws.Domain.Tests.Unit
{
    using System;
    using Xunit;

    public class OrganisationTests
    {
        private readonly Address address;
        private static readonly string AnyString = "test";

        public OrganisationTests()
        {
            this.address = new Address(AnyString, AnyString, AnyString, AnyString, AnyString, AnyString, AnyString);
        }

        [Fact]
        public void TypeOther_NoDescription_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new Organisation(AnyString, address, BusinessType.Other));
        }

        [Fact]
        public void TypeCompany_CreatesSuccessfully()
        {
            var organisation = new Organisation(AnyString, address, BusinessType.LimitedCompany, null);

            Assert.Equal(AnyString, organisation.Name);
            Assert.Equal(BusinessType.LimitedCompany.DisplayName, organisation.Type);
        }
    }
}
