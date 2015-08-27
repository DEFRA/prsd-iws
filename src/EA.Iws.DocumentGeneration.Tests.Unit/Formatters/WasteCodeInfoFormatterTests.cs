namespace EA.Iws.DocumentGeneration.Tests.Unit.Formatters
{
    using System;
    using Core.WasteCodes;
    using DocumentGeneration.Formatters;
    using Domain.NotificationApplication;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class WasteCodeInfoFormatterTests
    {
        private const string AnyString = "Any string";
        private const string AnyCode = "AC0D3";
        private readonly WasteCodeInfoFormatter formatter;

        public WasteCodeInfoFormatterTests()
        {
            formatter = new WasteCodeInfoFormatter();
        }

        [Fact]
        public void CodeListToString_Empty_ReturnsEmptyString()
        {
            var result = formatter.CodeListToString(new WasteCodeInfo[] { });

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void CodeListToString_Null_ReturnsEmptyString()
        {
            var result = formatter.CodeListToString(null);

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void CodeListToString_NotApplicable_ReturnsNotApplicable()
        {
            var result = formatter.CodeListToString(new[]
            {
                TestableWasteCodeInfo.Create(CodeType.Basel, null, null, true)
            });

            Assert.Equal(WasteCodeInfoFormatter.NotApplicable, result);
        }

        [Fact]
        public void CodeListToString_OneCode_ReturnsThatCode()
        {
            var result = formatter.CodeListToString(new[]
            {
                TestableWasteCodeInfo.Create(CodeType.Ewc, AnyCode)
            });

            Assert.Equal(AnyCode, result);
        }

        [Fact]
        public void CodeListToString_TwoCodes_ReturnsCodeList()
        {
            var result = formatter.CodeListToString(new[]
            {
                TestableWasteCodeInfo.Create(CodeType.Basel, AnyCode + 1),
                TestableWasteCodeInfo.Create(CodeType.Basel, AnyCode + 2)
            });

            Assert.Equal(string.Format("{0}1, {0}2", AnyCode), result);
        }

        [Fact]
        public void CodeListToString_ApplicableButWithNullCodes_Throws()
        {
            Assert.Throws<NullReferenceException>(() =>
                formatter.CodeListToString(new[]
                {
                    new TestableWasteCodeInfo
                    {
                        CodeType = CodeType.Ewc,
                        WasteCode = null
                    }
                }));
        }

        [Fact]
        public void GetValueOfCustomCode_NullCodeInfo_ReturnsEmptyString()
        {
            var result = formatter.GetCustomCodeValue(null);

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetValueOfCustomCode_NotApplicable_ReturnsNotApplicable()
        {
            var result = formatter.GetCustomCodeValue(TestableWasteCodeInfo.Create(CodeType.Ewc, null, null, true));

            Assert.Equal(WasteCodeInfoFormatter.NotApplicable, result);
        }

        [Fact]
        public void GetValueOfCustomCode_NullCustomCode_ReturnsEmptyString()
        {
            var result = formatter.GetCustomCodeValue(new TestableWasteCodeInfo
            {
                CodeType = CodeType.Ewc,
                CustomCode = null,
                WasteCode = null
            });

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetValueOfCustomCode_CustomCodeHasValue_ReturnsValue()
        {
            var result = formatter.GetCustomCodeValue(new TestableWasteCodeInfo
            {
                CodeType = CodeType.Ewc,
                CustomCode = AnyString
            });

            Assert.Equal(AnyString, result);
        }
    }
}
