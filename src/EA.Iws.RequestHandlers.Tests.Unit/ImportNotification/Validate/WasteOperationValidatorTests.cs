namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using System;
    using Core.ImportNotification.Draft;
    using FluentValidation.TestHelper;
    using RequestHandlers.ImportNotification.Validate;
    using Xunit;

    public class WasteOperationValidatorTests
    {
        private readonly Guid importNotificationId = new Guid("9C117E73-FB2D-4F9B-AEF6-26908AEAEFCD");
        private readonly WasteOperationValidator validator;

        public WasteOperationValidatorTests()
        {
            validator = new WasteOperationValidator();
        }

        [Fact]
        public void ValidWasteOperation_ResultIsValid()
        {
            var wasteOperation = GetValidWasteOperation();

            var result = validator.Validate(wasteOperation);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void HasDisposalAndRecoveryCodes_HasValidationError()
        {
            var wasteOperation = GetValidWasteOperation();
            wasteOperation.OperationCodes = new[] { 1, 2, 3, 14, 15, 16 };

            validator.ShouldHaveValidationErrorFor(x => x.OperationCodes, wasteOperation);
        }

        [Fact]
        public void HasNoOperationCodes_ResultIsInvalid()
        {
            var wasteOperation = GetValidWasteOperation();
            wasteOperation.OperationCodes = new int[] { };

            validator.ShouldHaveValidationErrorFor(x => x.OperationCodes, wasteOperation);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void TechnologyEmployedNotEntered_ResultIsValid(string input)
        {
            var wasteOperation = GetValidWasteOperation();
            wasteOperation.TechnologyEmployed = input;

            validator.ShouldNotHaveValidationErrorFor(x => x.TechnologyEmployed, wasteOperation);
        }

        [Fact]
        public void OperationCodesNull_ResultIsInvalid()
        {
            var wasteOperation = GetValidWasteOperation();
            wasteOperation.OperationCodes = null;

            validator.ShouldHaveValidationErrorFor(x => x.OperationCodes, wasteOperation);
        }

        private WasteOperation GetValidWasteOperation()
        {
            return new WasteOperation(importNotificationId)
            {
                OperationCodes = new[] { 1, 2, 3 },
                TechnologyEmployed = "technology employed"
            };
        }
    }
}