namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using System;
    using System.Collections.Generic;
    using Core.ImportNotification.Draft;
    using FluentValidation.TestHelper;
    using RequestHandlers.ImportNotification.Validate;
    using Xunit;

    public class WasteTypeValidatorTests
    {
        private readonly WasteTypeValidator validator;

        public WasteTypeValidatorTests()
        {
            validator = new WasteTypeValidator();
        }

        [Fact]
        public void ValidWasteTypeWithCodes_HasNoValidationErrors()
        {
            var validWasteType = new WasteType
            {
                Name = "Waste",
                SelectedBaselCode = Guid.NewGuid(),
                SelectedEwcCodes = new List<Guid>() { Guid.NewGuid() },
                SelectedHCodes = new List<Guid>() { Guid.NewGuid() },
                SelectedUnClasses = new List<Guid>() { Guid.NewGuid() },
                SelectedYCodes = new List<Guid>() { Guid.NewGuid() },
            };

            var result = validator.Validate(validWasteType);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void ValidWasteTypeWithoutCodes_HasNoValidationErrors()
        {
            var validWasteType = new WasteType
            {
                Name = "Waste",
                SelectedEwcCodes = new List<Guid> { Guid.NewGuid() },
                BaselCodeNotListed = true,
                HCodeNotApplicable = true,
                UnClassNotApplicable = true,
                YCodeNotApplicable = true
            };

            var result = validator.Validate(validWasteType);

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void InvalidName_HasValidationError(string input)
        {
            validator.ShouldHaveValidationErrorFor(x => x.Name, input);
        }

        [Fact]
        public void InvalidSelectedBaselCode_HasValidationError()
        {
            var wasteType = new WasteType
            {
                BaselCodeNotListed = false,
                SelectedBaselCode = null
            };

            validator.ShouldHaveValidationErrorFor(x => x.SelectedBaselCode, wasteType);
        }

        [Fact]
        public void InvalidSelectedEwcCodes_HasValidationError()
        {
            var wasteType = new WasteType
            {
                SelectedEwcCodes = new List<Guid>()
            };

            validator.ShouldHaveValidationErrorFor(x => x.SelectedEwcCodes, wasteType);
        }

        [Fact]
        public void InvalidSelectedHCodes_HasValidationError()
        {
            var wasteType = new WasteType
            {
                HCodeNotApplicable = false,
                SelectedHCodes = new List<Guid>()
            };

            validator.ShouldHaveValidationErrorFor(x => x.SelectedHCodes, wasteType);
        }

        [Fact]
        public void InvalidSelectedUnClasses_HasValidationError()
        {
            var wasteType = new WasteType
            {
                UnClassNotApplicable = false,
                SelectedUnClasses = new List<Guid>()
            };

            validator.ShouldHaveValidationErrorFor(x => x.SelectedUnClasses, wasteType);
        }

        [Fact]
        public void InvalidSelectedYCodes_HasValidationError()
        {
            var wasteType = new WasteType
            {
                YCodeNotApplicable = false,
                SelectedYCodes = new List<Guid>()
            };

            validator.ShouldHaveValidationErrorFor(x => x.SelectedYCodes, wasteType);
        }

        [Fact]
        public void BaselCodeNotListed_HasNoValidationError()
        {
            var wasteType = new WasteType
            {
                BaselCodeNotListed = true,
                SelectedBaselCode = null
            };

            validator.ShouldNotHaveValidationErrorFor(x => x.SelectedBaselCode, wasteType);
        }

        [Fact]
        public void HCodeNotApplicable_HasNoValidationError()
        {
            var wasteType = new WasteType
            {
                HCodeNotApplicable = true,
                SelectedHCodes = new List<Guid>()
            };

            validator.ShouldNotHaveValidationErrorFor(x => x.SelectedHCodes, wasteType);
        }

        [Fact]
        public void UnClassesNotApplicable_HasNoValidationError()
        {
            var wasteType = new WasteType
            {
                UnClassNotApplicable = true,
                SelectedUnClasses = new List<Guid>()
            };

            validator.ShouldNotHaveValidationErrorFor(x => x.SelectedUnClasses, wasteType);
        }

        [Fact]
        public void YCodeNotApplicable_HasNoValidationError()
        {
            var wasteType = new WasteType
            {
                YCodeNotApplicable = true,
                SelectedYCodes = new List<Guid>()
            };

            validator.ShouldNotHaveValidationErrorFor(x => x.SelectedYCodes, wasteType);
        }
    }
}