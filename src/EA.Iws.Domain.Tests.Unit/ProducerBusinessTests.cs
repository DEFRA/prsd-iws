namespace EA.Iws.Domain.Tests.Unit
{
    using System;
    using Domain.NotificationApplication;
    using Xunit;

    public class ProducerBusinessTests
    {
        private static readonly string AnyString = "test";
        private static readonly string NotApplicable = "Not applicable";

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void Create_LimitedCompanyWithNullOrEmptyNumber_Throws(string registrationNumber)
        {
            Assert.Throws<InvalidOperationException>(
                () =>
                    ProducerBusiness.CreateProducerBusiness(AnyString, BusinessType.LimitedCompany, registrationNumber,
                        null));
        }

        [Fact]
        public void Create_OtherWithEmptyDescription_Throws()
        {
            Assert.Throws<ArgumentException>(
                () => ProducerBusiness.CreateProducerBusiness(AnyString, BusinessType.Other, null, string.Empty));
        }

        [Fact]
        public void Create_OtherWithNullDescription_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => ProducerBusiness.CreateProducerBusiness(AnyString, BusinessType.Other, null, null));
        }

        [Fact]
        public void Create_SoleTrader_NonNullRegistrationNumber_DoesNotSetToCustomValue()
        {
            var business = ProducerBusiness.CreateProducerBusiness(AnyString, BusinessType.SoleTrader, AnyString, null);

            Assert.Equal(NotApplicable, business.RegistrationNumber);
        }

        [Fact]
        public void Create_LimitedCompany_setsRegistrationNumber()
        {
            var number = "554636546";

            var business = ProducerBusiness.CreateProducerBusiness(AnyString, BusinessType.LimitedCompany, number, null);

            Assert.Equal(number, business.RegistrationNumber);
        }
    }
}
