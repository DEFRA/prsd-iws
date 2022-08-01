﻿namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using System;
    using System.Collections.Generic;
    using Core.ImportNotification.Draft;
    using FluentValidation.TestHelper;
    using RequestHandlers.ImportNotification.Validate;
    using Xunit;

    public class WasteTypeValidatorTests
    {
        private static readonly Guid AnyGuid = new Guid("DF341A3F-27BF-4453-A7B5-710059EF1165");
        private readonly WasteTypeValidator validator;

        public WasteTypeValidatorTests()
        {
            validator = new WasteTypeValidator();
        }

        [Fact]
        public void ValidWasteTypeWithCodes_HasNoValidationErrors()
        {
            var validWasteType = new WasteType(AnyGuid)
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
            var validWasteType = new WasteType(AnyGuid)
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
            var wasteType = new WasteType(AnyGuid);
            wasteType.Name = input;

            var result = validator.TestValidate(wasteType);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void InvalidSelectedBaselCode_HasValidationError()
        {
            var wasteType = new WasteType(AnyGuid)
            {
                BaselCodeNotListed = false,
                SelectedBaselCode = null
            };

            var result = validator.TestValidate(wasteType);
            result.ShouldHaveValidationErrorFor(x => x.SelectedBaselCode);
        }

        [Fact]
        public void InvalidSelectedEwcCodes_HasValidationError()
        {
            var wasteType = new WasteType(AnyGuid)
            {
                SelectedEwcCodes = new List<Guid>()
            };

            var result = validator.TestValidate(wasteType);
            result.ShouldHaveValidationErrorFor(x => x.SelectedEwcCodes);
        }

        [Fact]
        public void InvalidSelectedHCodes_HasValidationError()
        {
            var wasteType = new WasteType(AnyGuid)
            {
                HCodeNotApplicable = false,
                SelectedHCodes = new List<Guid>()
            };

            var result = validator.TestValidate(wasteType);
            result.ShouldHaveValidationErrorFor(x => x.SelectedHCodes);
        }

        [Fact]
        public void InvalidSelectedUnClasses_HasValidationError()
        {
            var wasteType = new WasteType(AnyGuid)
            {
                UnClassNotApplicable = false,
                SelectedUnClasses = new List<Guid>()
            };

            var result = validator.TestValidate(wasteType);
            result.ShouldHaveValidationErrorFor(x => x.SelectedUnClasses);
        }

        [Fact]
        public void InvalidSelectedYCodes_HasValidationError()
        {
            var wasteType = new WasteType(AnyGuid)
            {
                YCodeNotApplicable = false,
                SelectedYCodes = new List<Guid>()
            };

            var result = validator.TestValidate(wasteType);
            result.ShouldHaveValidationErrorFor(x => x.SelectedYCodes);
        }

        [Fact]
        public void BaselCodeNotListed_HasNoValidationError()
        {
            var wasteType = new WasteType(AnyGuid)
            {
                BaselCodeNotListed = true,
                SelectedBaselCode = null
            };

            var result = validator.TestValidate(wasteType);
            result.ShouldHaveValidationErrorFor(x => x.SelectedBaselCode);
        }

        [Fact]
        public void HCodeNotApplicable_HasNoValidationError()
        {
            var wasteType = new WasteType(AnyGuid)
            {
                HCodeNotApplicable = true,
                SelectedHCodes = new List<Guid>()
            };

            var result = validator.TestValidate(wasteType);
            result.ShouldHaveValidationErrorFor(x => x.SelectedHCodes);
        }

        [Fact]
        public void UnClassesNotApplicable_HasNoValidationError()
        {
            var wasteType = new WasteType(AnyGuid)
            {
                UnClassNotApplicable = true,
                SelectedUnClasses = new List<Guid>()
            };

            var result = validator.TestValidate(wasteType);
            result.ShouldHaveValidationErrorFor(x => x.SelectedUnClasses);
        }

        [Fact]
        public void YCodeNotApplicable_HasNoValidationError()
        {
            var wasteType = new WasteType(AnyGuid)
            {
                YCodeNotApplicable = true,
                SelectedYCodes = new List<Guid>()
            };

            var result = validator.TestValidate(wasteType);
            result.ShouldHaveValidationErrorFor(x => x.SelectedYCodes);
        }
    }
}