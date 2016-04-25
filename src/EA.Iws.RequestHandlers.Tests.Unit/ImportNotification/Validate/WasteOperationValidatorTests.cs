namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using System;
    using Core.ImportNotification.Draft;
    using Core.OperationCodes;
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
            wasteOperation.OperationCodes = new[] { OperationCode.D1, OperationCode.D2, OperationCode.D3, OperationCode.R1, OperationCode.R2, OperationCode.R3 };

            validator.ShouldHaveValidationErrorFor(x => x.OperationCodes, wasteOperation);
        }

        [Fact]
        public void HasNoOperationCodes_ResultIsInvalid()
        {
            var wasteOperation = GetValidWasteOperation();
            wasteOperation.OperationCodes = new OperationCode[] { };

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
        public void TechnologyEmployedGreaterThan70_ResultIsInvalid()
        {
            var wasteOperation = GetValidWasteOperation();
            wasteOperation.TechnologyEmployed =
                "123245678901232456789012324567890123245678901232456789012324567890123245678901232456789012324567890123245678901";

            validator.ShouldHaveValidationErrorFor(x => x.TechnologyEmployed, wasteOperation);
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
                OperationCodes = new[] { OperationCode.D1, OperationCode.D2, OperationCode.D3 },
                TechnologyEmployed = "technology employed"
            };
        }
    }
}