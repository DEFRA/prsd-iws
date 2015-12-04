namespace EA.Iws.RequestHandlers.Tests.Unit.Mappings
{
    using System.Collections.Generic;
    using Domain;
    using Domain.NotificationApplication;
    using RequestHandlers.Mappings;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class BusinessMapTests
    {
        private const string AnyString = "test";
        private const string TestString = "micro pig";

        private readonly BusinessMap businessMap;
        private readonly TestableBusiness testBusiness = new TestableBusiness
        {
            Name = AnyString,
            AdditionalRegistrationNumber = AnyString,
            RegistrationNumber = AnyString,
            Type = BusinessType.Partnership
        };

        public BusinessMapTests()
        {
            businessMap = new BusinessMap();
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
        [MemberData("GetDataForBusinessMapTests")]
        public void MapEntityType(BusinessType entityType)
        {
            testBusiness.Type = entityType;
            var result = businessMap.Map(testBusiness);
            Assert.Equal(entityType.DisplayName, result.EntityType);
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

        public static IEnumerable<object[]> GetDataForBusinessMapTests
        {
            get
            {
                yield return new object[] { BusinessType.LimitedCompany };
                yield return new object[] { BusinessType.SoleTrader };
                yield return new object[] { BusinessType.Partnership };
                yield return new object[] { BusinessType.Other };
            }
        }
    }
}