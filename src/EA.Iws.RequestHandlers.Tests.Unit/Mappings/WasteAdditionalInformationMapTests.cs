namespace EA.Iws.RequestHandlers.Tests.Unit.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.WasteType;
    using Domain.NotificationApplication;
    using RequestHandlers.Mappings;
    using Xunit;

    public class WasteAdditionalInformationMapTests
    {
        private readonly WasteAdditionalInformationMap map 
            = new WasteAdditionalInformationMap();
        private readonly IList<WoodInformationData> wasteData 
            = new List<WoodInformationData>();

        private static readonly WoodInformationData ChlorineData
            = new WoodInformationData
            {
                Constituent = "test",
                MaxConcentration = "100",
                MinConcentration = "0",
                WasteInformationType = WasteInformationType.Chlorine
            };

        private static readonly WoodInformationData AshData = 
            new WoodInformationData
            {
                Constituent = null,
                MaxConcentration = "2.3",
                MinConcentration = "1",
                WasteInformationType = WasteInformationType.AshContent
            };

        private static readonly WoodInformationData InvalidData =
            new WoodInformationData
            {
                Constituent = null,
                MaxConcentration = "NA",
                MinConcentration = "1",
                WasteInformationType = WasteInformationType.MoistureContent
            };

        [Fact]
        public void ConvertsEmptyInputListToEmptyOutput()
        {
            var result = map.Map(wasteData);

            Assert.Empty(result);
        }

        [Fact]
        public void ThrowsOnNullInput()
        {
            Assert.Throws<ArgumentNullException>(() => map.Map(null));
        }

        [Fact]
        public void ConvertsListWithData()
        {
            wasteData.Add(ChlorineData);

            var result = map.Map(wasteData);

            AssertEquality(ChlorineData, result.Single());
        }

        [Fact]
        public void ConvertsListWithMultipleItems()
        {
            wasteData.Add(ChlorineData);
            wasteData.Add(AshData);

            var result = map.Map(wasteData);

            AssertEquality(ChlorineData, result.Single(r => r.WasteInformationType == WasteInformationType.Chlorine));
            AssertEquality(AshData, result.Single(r => r.WasteInformationType == WasteInformationType.AshContent));
        }

        [Fact]
        public void ThrowsOnInvalidCast()
        {
            wasteData.Add(InvalidData);

            Assert.Throws<FormatException>(() => map.Map(wasteData));
        }

        private void AssertEquality(WoodInformationData expected, 
            WasteAdditionalInformation actual)
        {
            Assert.Equal(expected.WasteInformationType, actual.WasteInformationType);
            Assert.Equal(Convert.ToDecimal(expected.MinConcentration), actual.MinConcentration);
            Assert.Equal(Convert.ToDecimal(expected.MaxConcentration), actual.MaxConcentration);
            Assert.Equal(expected.Constituent, actual.Constituent);
        }
    }
}
