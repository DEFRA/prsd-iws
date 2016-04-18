namespace EA.Iws.DocumentGeneration.Tests.Unit.ViewModels
{
    using System.Collections.Generic;
    using Core.WasteType;
    using DocumentGeneration.Formatters;
    using DocumentGeneration.ViewModels;
    using Domain.NotificationApplication;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class WasteCompositionViewModelTests
    {
        private readonly TestableWasteType wasteType = new TestableWasteType();
        private readonly WasteCompositionFormatter wasteCompositionFormatter = new WasteCompositionFormatter();

        [Fact]
        public void ConstructAsOtherType_ShortDescription_AnnexMessageEmpty()
        {
            wasteType.ChemicalCompositionType = ChemicalComposition.Other;
            wasteType.OtherWasteTypeDescription = "a short description";

            var first = new WasteCompositionViewModel(wasteType, wasteCompositionFormatter);
            var result = new WasteCompositionViewModel(first, 7);

            Assert.Equal(string.Empty, result.AnnexMessage);
            Assert.Equal(wasteType.OtherWasteTypeDescription, result.ShortDescription);
            Assert.Equal(string.Empty, result.LongDescription);
        }

        [Fact]
        public void ConstructAsOtherType_LongDescription_AnnexMessageShown()
        {
            wasteType.ChemicalCompositionType = ChemicalComposition.Other;
            wasteType.OtherWasteTypeDescription = new string('a', 250);

            var first = new WasteCompositionViewModel(wasteType, wasteCompositionFormatter);
            var result = new WasteCompositionViewModel(first, 7);

            Assert.Equal("See Annex 7", result.AnnexMessage);
            Assert.Equal(string.Empty, result.ShortDescription);
            Assert.Equal(wasteType.OtherWasteTypeDescription, result.LongDescription);
        }

        [Fact]
        public void ConstructAsRdfType_AnnexMessageShown()
        {
            wasteType.ChemicalCompositionType = ChemicalComposition.RDF;
            wasteType.WasteCompositions = new List<WasteComposition>
            {
                WasteComposition.CreateWasteComposition("test", 1, 5, ChemicalCompositionCategory.Food)
            };

            var first = new WasteCompositionViewModel(wasteType, wasteCompositionFormatter);
            var result = new WasteCompositionViewModel(first, 7);

            Assert.Equal("See Annex 7", result.AnnexMessage);
        }
    }
}
