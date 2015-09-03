namespace EA.Iws.RequestHandlers.Tests.Unit.Mappings
{
    using System;
    using Core.Shared;
    using RequestHandlers.Mappings;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class BusinessInfoMapTests
    {
        private const string AnyString = "test";
        private const string TestString = "micro pig";

        private readonly BusinessInfoMap businessMap;
        private readonly TestableBusiness testBusiness = new TestableBusiness
        {
            Name = AnyString,
            AdditionalRegistrationNumber = AnyString,
            RegistrationNumber = AnyString,
            Type = "Partnership"
        };

        public BusinessInfoMapTests()
        {
            businessMap = new BusinessInfoMap();
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(TestString)]
        public void MapName(string name)
        {
            testBusiness.Name = name;

            var result = businessMap.Map(testBusiness);

            Assert.Equal(name, result.Name);
        }

        [Theory]
        [InlineData("Limited company", BusinessType.LimitedCompany)]
        [InlineData("Other", BusinessType.Other)]
        [InlineData("Sole trader", BusinessType.SoleTrader)]
        [InlineData("Partnership", BusinessType.Partnership)]
        public void MapBusinessType(string entityType, BusinessType businessType)
        {
            testBusiness.Type = entityType;

            var result = businessMap.Map(testBusiness);

            Assert.Equal(businessType, result.BusinessType);
        }

        [Fact]
        public void InvalidBusinessTypeThrows()
        {
            testBusiness.Type = TestString;

            Assert.Throws<ArgumentException>(() => businessMap.Map(testBusiness));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(TestString)]
        public void MapRegistrationNumber(string registrationNumber)
        {
            testBusiness.RegistrationNumber = registrationNumber;

            var result = businessMap.Map(testBusiness);

            Assert.Equal(registrationNumber, result.RegistrationNumber);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(TestString)]
        public void MapAdditionalRegistrationNumber(string additionalRegistrationNumber)
        {
            testBusiness.AdditionalRegistrationNumber = additionalRegistrationNumber;

            var result = businessMap.Map(testBusiness);

            Assert.Equal(additionalRegistrationNumber, result.AdditionalRegistrationNumber);
        }
    }
}