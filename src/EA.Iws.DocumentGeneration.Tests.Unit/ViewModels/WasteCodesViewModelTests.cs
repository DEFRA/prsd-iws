namespace EA.Iws.DocumentGeneration.Tests.Unit.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Core.WasteCodes;
    using DocumentGeneration.Formatters;
    using DocumentGeneration.ViewModels;
    using Domain.NotificationApplication;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class WasteCodesViewModelTests
    {
        private const string NotApplicable = "Not applicable";
        private const string NotListed = "Not listed";
        private const string AnyString = "Any string";
        private const string AnyCode = "AC0D3";

        private readonly TestableWasteCodesViewModel viewModel;
        private readonly TestableNotificationApplication notification;
        private readonly List<WasteCodeInfo> wasteCodes = new List<WasteCodeInfo>(); 

        public WasteCodesViewModelTests()
        {
            notification = new TestableNotificationApplication
            {
                WasteCodes = wasteCodes
            };

            viewModel = FormatterServices.GetUninitializedObject(typeof(TestableWasteCodesViewModel)) as TestableWasteCodesViewModel;
        }

        [Theory]
        [InlineData(CodeType.Basel)]
        [InlineData(CodeType.Oecd)]
        public void BaselAndOecdCode_IsNotApplicable_ReturnsNotApplicable(CodeType codeType)
        {
            wasteCodes.Add(TestableWasteCodeInfo.Create(codeType, null, null, true));

            viewModel.SetBaselAndOecdCode(notification);

            var result = (codeType == CodeType.Basel) ? viewModel.Basel : viewModel.Oecd;
            var other = (codeType == CodeType.Basel) ? viewModel.Oecd : viewModel.Basel;

            Assert.Equal(NotListed, result);
            Assert.Equal(string.Empty, other);
        }

        [Theory]
        [InlineData(CodeType.Basel)]
        [InlineData(CodeType.Oecd)]
        public void BaselAndOecdCode_HasCode_ReturnsCode(CodeType codeType)
        {
            wasteCodes.Add(TestableWasteCodeInfo.Create(codeType, AnyCode));

            viewModel.SetBaselAndOecdCode(notification);

            var result = (codeType == CodeType.Basel) ? viewModel.Basel : viewModel.Oecd;
            var other = (codeType == CodeType.Basel) ? viewModel.Oecd : viewModel.Basel;

            Assert.Equal(AnyCode, result);
            Assert.Equal(string.Empty, other);
        }

        [Fact]
        public void BaselAndOecdCodeIsNull_ReturnsEmptyString()
        {
            viewModel.SetBaselAndOecdCode(notification);

            Assert.Equal(string.Empty, viewModel.Basel);
            Assert.Equal(string.Empty, viewModel.Oecd);
        }

        [Fact]
        public void SetUnNumbersAndShippingName_NotApplicable_ReturnsUnNumberNotApplicable()
        {
            wasteCodes.Add(TestableWasteCodeInfo.Create(CodeType.UnNumber, null, null, true));

            viewModel.SetUnNumbersAndShippingNames(notification);

            Assert.Equal(NotApplicable, viewModel.UnNumber);
        }

        [Fact]
        public void SetUnNumbersAndShippingName_NotApplicable_ReturnsUnShippingNameEmpty()
        {
            wasteCodes.Add(TestableWasteCodeInfo.Create(CodeType.UnNumber, null, null, true));
            
            viewModel.SetUnNumbersAndShippingNames(notification);
            
            Assert.Equal(string.Empty, viewModel.UnShippingName);
        }

        [Fact]
        public void SetUnNumbersAndShippingName_NotApplicable_ReturnsUnShippingNumberAndNameNotApplicable()
        {
            wasteCodes.Add(TestableWasteCodeInfo.Create(CodeType.UnNumber, null, null, true));

            viewModel.SetUnNumbersAndShippingNames(notification);

            Assert.Equal(NotApplicable, viewModel.UnNumberAndShippingName);
        }

        [Fact]
        public void SetUnNumbersAndShippingName_OneUnNumber_ReturnsUnNumber()
        {
            wasteCodes.Add(TestableWasteCodeInfo.Create(CodeType.UnNumber, AnyCode, AnyString));

            viewModel.SetUnNumbersAndShippingNames(notification);

            Assert.Equal(AnyCode, viewModel.UnNumber);
        }

        [Fact]
        public void SetUnNumbersAndShippingName_OneUnNumber_ReturnsUnShippingName()
        {
            wasteCodes.Add(TestableWasteCodeInfo.Create(CodeType.UnNumber, AnyCode, AnyString));

            viewModel.SetUnNumbersAndShippingNames(notification);

            Assert.Equal(AnyString, viewModel.UnShippingName);
        }

        [Fact]
        public void SetUnNumbersAndShippingName_OneUnNumber_ReturnsUnShippingNumberAndName()
        {
            wasteCodes.Add(TestableWasteCodeInfo.Create(CodeType.UnNumber, AnyCode, AnyString));

            viewModel.SetUnNumbersAndShippingNames(notification);

            Assert.Equal(string.Format("{0} - {1}", AnyCode, AnyString),
                viewModel.UnNumberAndShippingName);
        }

        [Fact]
        public void SetUnNumbersAndShippingName_Empty_ReturnsUnNumberEmptyString()
        {
            viewModel.SetUnNumbersAndShippingNames(notification);

            Assert.Equal(string.Empty, viewModel.UnNumber);
        }

        [Fact]
        public void SetUnNumbersAndShippingName_Empty_ReturnsUnShippingNameEmptyString()
        {
            viewModel.SetUnNumbersAndShippingNames(notification);

            Assert.Equal(string.Empty, viewModel.UnShippingName);
        }

        [Fact]
        public void SetUnNumbersAndShippingName_Empty_ReturnsUnShippingNumberAndNameEmptyString()
        {
            viewModel.SetUnNumbersAndShippingNames(notification);

            Assert.Equal(string.Empty, viewModel.UnNumberAndShippingName);
        }

        [Fact]
        public void SetUnNumbersAndShippingName_TwoUnNumbers_ReturnsNumberList()
        {
            wasteCodes.AddRange(new[] 
            {
                TestableWasteCodeInfo.Create(CodeType.UnNumber, AnyCode + "1", AnyString + "1"),
                TestableWasteCodeInfo.Create(CodeType.UnNumber, AnyCode + "2", AnyString + "2")
            });

            viewModel.SetUnNumbersAndShippingNames(notification);

            var expectedCodesString = string.Format("{0}1, {0}2", AnyCode);
            var expectedDescriptionsString = string.Format("{0}1, {0}2", AnyString);
            var expectedNumberAndShippingNameString =
                string.Format("{0}1 - {1}1{2}{0}2 - {1}2", AnyCode, AnyString, Environment.NewLine);

            Assert.Equal(expectedCodesString, viewModel.UnNumber);
            Assert.Equal(expectedDescriptionsString, viewModel.UnShippingName);
            Assert.Equal(expectedNumberAndShippingNameString, viewModel.UnNumberAndShippingName);
        }

        private class TestableWasteCodesViewModel : WasteCodesViewModel
        {
            public TestableWasteCodesViewModel(NotificationApplication notification) 
                : base(notification, new WasteCodeInfoFormatter())
            {
            }

            public new AnnexTableWasteCodes AddToAnnexTableWasteCodes(string name, string codes)
            {
                return base.AddToAnnexTableWasteCodes(name, codes);
            }

            public new void SetBaselAndOecdCode(NotificationApplication notification)
            {
                base.SetBaselAndOecdCode(notification);
            }

            public new void SetUnNumbersAndShippingNames(NotificationApplication notification)
            {
                base.SetUnNumbersAndShippingNames(notification);
            }
        }
    }
}
