namespace EA.Iws.RequestHandlers.Tests.Unit.Mappings
{
    using System;
    using Core.WasteCodes;
    using Domain.NotificationApplication;
    using RequestHandlers.Mappings;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class WasteCodeMapTests
    {
        private static readonly Guid AnyGuid = new Guid("9F3D2CEB-C047-4C87-BF38-533F73359A53");
        private static readonly CodeType AnyCodeType = CodeType.Y;
        private static readonly string AnyString = "Any string";

        private static readonly WasteCodeInfo NotApplicableNullWasteCode;
        private static readonly WasteCodeInfo HasWasteCode;
        private static readonly WasteCodeInfo CustomCode;

        private readonly WasteCodeMap wasteCodeMap;

        static WasteCodeMapTests()
        {
            NotApplicableNullWasteCode = new TestableWasteCodeInfo
            {
                IsNotApplicable = true,
                CodeType = AnyCodeType
            };

            HasWasteCode = new TestableWasteCodeInfo
            {
                CodeType = AnyCodeType,
                WasteCode = new TestableWasteCode
                {
                    Id = AnyGuid,
                    CodeType = AnyCodeType,
                    Code = AnyString
                }
            };

            CustomCode = new TestableWasteCodeInfo
            {
                CodeType = AnyCodeType,
                CustomCode = AnyString
            };
        }

        public WasteCodeMapTests()
        {
            wasteCodeMap = new WasteCodeMap();
        }

        [Fact]
        public void Info_HasWasteCode()
        {
            var result = wasteCodeMap.Map(HasWasteCode);

            Assert.Equal(HasWasteCode.WasteCode.Id, result.Id);
            Assert.Equal(HasWasteCode.WasteCode.Code, result.Code);
        }

        [Fact]
        public void Info_HasCustomCode()
        {
            var result = wasteCodeMap.Map(CustomCode);

            Assert.Equal(CustomCode.CustomCode, result.Code);
            Assert.Equal(CustomCode.CustomCode, result.CustomCode);
            Assert.Equal(CustomCode.CodeType, result.CodeType);
            Assert.Equal(Guid.Empty, result.Id);
        }

        [Fact]
        public void Info_NotApplicable()
        {
            var result = wasteCodeMap.Map(NotApplicableNullWasteCode);

            Assert.Equal(Guid.Empty, result.Id);
            Assert.Equal(NotApplicableNullWasteCode.CodeType, result.CodeType);
            Assert.True(result.IsNotApplicable);
            Assert.Null(result.Code);
            Assert.Null(result.Description);
        }
    }
}
