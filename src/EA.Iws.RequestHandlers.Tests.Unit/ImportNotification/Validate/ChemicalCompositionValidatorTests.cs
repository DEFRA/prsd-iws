namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using System;
    using Core.ImportNotification.Draft;
    using FluentValidation.TestHelper;
    using RequestHandlers.ImportNotification.Validate;
    using Xunit;

    public class ChemicalCompositionValidatorTests
    {
        private static readonly Guid AnyGuid = new Guid("DF341A3F-27BF-4453-A7B5-710059EF1165");
        private readonly ChemicalCompositionValidator validator;

        public ChemicalCompositionValidatorTests()
        {
            validator = new ChemicalCompositionValidator();
        }

        [Fact]
        public void ValidChemicalComposition_HasNoValidationErrors()
        {
            var validChemicalComposition = new ChemicalComposition(AnyGuid)
            {
                Composition = Core.WasteType.ChemicalComposition.Other
            };

            var result = validator.Validate(validChemicalComposition);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void NullChemicalComposition_HasValidationError()
        {
            var chemicalComposition = new ChemicalComposition(AnyGuid)
            {
                Composition = null
            };

            validator.ShouldHaveValidationErrorFor(x => x.Composition, chemicalComposition);
        }

        [Fact]
        public void DefaultChemicalComposition_HasValidationError()
        {
            var chemicalComposition = new ChemicalComposition(AnyGuid)
            {
                Composition = default(Core.WasteType.ChemicalComposition)
            };

            validator.ShouldHaveValidationErrorFor(x => x.Composition, chemicalComposition);
        }
    }
}