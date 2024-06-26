namespace EA.Iws.RequestHandlers.Tests.Unit.Mappings
{
    using System.Collections.Generic;
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
            Type = BusinessType.Partnership
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
        [MemberData(memberName: nameof(GetDataForBusinessInfoMapTests))]
        public void MapBusinessType(BusinessType entityType, BusinessType businessType)
        {
            testBusiness.Type = entityType;
            var result = businessMap.Map(testBusiness);
            Assert.Equal(businessType, result.BusinessType);
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

        public static IEnumerable<object[]> GetDataForBusinessInfoMapTests
        {
            get
            {
                yield return new object[] { BusinessType.LimitedCompany, BusinessType.LimitedCompany };
                yield return new object[] { BusinessType.SoleTrader, BusinessType.SoleTrader };
                yield return new object[] { BusinessType.Partnership, BusinessType.Partnership };
                yield return new object[] { BusinessType.Other, BusinessType.Other };
            }
        }
    }
}