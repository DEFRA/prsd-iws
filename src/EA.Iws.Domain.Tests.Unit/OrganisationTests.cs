namespace EA.Iws.Domain.Tests.Unit
{
    using System;
    using Xunit;

    public class OrganisationTests
    {
        private const string AnyString = "test";

        [Fact]
        public void TypeOther_NoDescription_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new Organisation(AnyString, BusinessType.Other));
        }

        [Fact]
        public void TypeCompany_CreatesSuccessfully()
        {
            var organisation = new Organisation(AnyString, BusinessType.LimitedCompany, null);

            Assert.Equal(AnyString, organisation.Name);
            Assert.Equal(BusinessType.LimitedCompany, organisation.Type);
        }
    }
}
