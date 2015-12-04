namespace EA.Iws.Domain.Tests.Unit
{
    using System;
    using Domain.NotificationApplication;
    using Xunit;

    public class BusinessTests
    {
        [Fact]
        public void CanCreateBusiness()
        {
            var business = Business.CreateBusiness("name", BusinessType.LimitedCompany, "123", "456");

            Assert.NotNull(business);
        }

        [Fact]
        public void CanCreateOtherBusiness()
        {
            var business = Business.CreateOtherBusiness("name", "123", "456", "other");

            Assert.NotNull(business);
        }

        [Fact]
        public void CantCreateOtherBusinessFromCreateBusinessFactory()
        {
            Action createBusiness = () => Business.CreateBusiness("name", BusinessType.Other, "123", "456");

            Assert.Throws<InvalidOperationException>(createBusiness);
        }

        [Fact]
        public void CreateOtherBusinessSetsBusinessTypeOther()
        {
            var business = Business.CreateOtherBusiness("name", "123", "456", "other");

            Assert.Equal(BusinessType.Other, business.Type);
        }

        [Fact]
        public void NameCannotBeNull()
        {
            Action createBusiness = () => Business.CreateBusiness(null, BusinessType.LimitedCompany, "123", "456");

            Assert.Throws<ArgumentNullException>("name", createBusiness);
        }

        [Fact]
        public void NameCannotBeEmpty()
        {
            Action createBusiness = () => Business.CreateBusiness(string.Empty, BusinessType.LimitedCompany, "123", "456");

            Assert.Throws<ArgumentException>("name", createBusiness);
        }

        [Fact]
        public void RegistrationNumberCannotBeNull()
        {
            Action createBusiness = () => Business.CreateBusiness("name", BusinessType.LimitedCompany, null, "456");

            Assert.Throws<ArgumentNullException>("registrationNumber", createBusiness);
        }

        [Fact]
        public void RegistrationNumberCannotBeEmpty()
        {
            Action createBusiness = () => Business.CreateBusiness("name", BusinessType.LimitedCompany, string.Empty, "456");

            Assert.Throws<ArgumentException>("registrationNumber", createBusiness);
        }

        [Fact]
        public void AdditionalRegistrationNumberCanBeNull()
        {
            var business = Business.CreateBusiness("name", BusinessType.LimitedCompany, "123", null);

            Assert.NotNull(business);
        }

        [Fact]
        public void AdditionalRegistrationNumberCanBeEmpty()
        {
            var business = Business.CreateBusiness("name", BusinessType.LimitedCompany, "123", string.Empty);

            Assert.NotNull(business);
        }

        [Fact]
        public void OtherDescriptionCannotBeNull()
        {
            Action createBusiness = () => Business.CreateOtherBusiness("name", "123", "456", null);

            Assert.Throws<ArgumentNullException>("value", createBusiness);
        }

        [Fact]
        public void OtherDescriptionCannotBeEmpty()
        {
            Action createBusiness = () => Business.CreateOtherBusiness("name", "123", "456", string.Empty);

            Assert.Throws<ArgumentException>("value", createBusiness);
        }
    }
}