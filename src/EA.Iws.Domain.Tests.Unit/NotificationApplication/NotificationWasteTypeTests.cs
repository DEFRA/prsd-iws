namespace EA.Iws.Domain.Tests.Unit.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Shared;
    using Core.WasteType;
    using Domain.NotificationApplication;
    using Xunit;

    public class NotificationWasteTypeTests
    {
        private readonly NotificationApplication notification;

        public NotificationWasteTypeTests()
        {
            notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);
        }

        [Fact]
        public void CanAddWasteTypeForWood()
        {
            notification.SetWasteType(WasteType.CreateRdfWasteType(GetWasteAdditionalInformationCollection()));

            Assert.True(notification.HasWasteType);
        }

        [Fact]
        public void CanAddWasteTypeForOther()
        {
            notification.SetWasteType(WasteType.CreateRdfWasteType(GetWasteAdditionalInformationCollection()));

            Assert.True(notification.HasWasteType);
        }

        [Fact]
        public void CanAddWasteTypeForRdf()
        {
            notification.SetWasteType(WasteType.CreateRdfWasteType(GetWasteAdditionalInformationCollection()));

            Assert.True(notification.HasWasteType);
            Assert.Equal(5, notification.WasteType.WasteAdditionalInformation.Count());
        }

        [Fact]
        public void CanAddWasteTypeForSrf()
        {
            notification.SetWasteType(WasteType.CreateSrfWasteType(GetWasteAdditionalInformationCollection()));

            Assert.True(notification.HasWasteType);
            Assert.Equal(5, notification.WasteType.WasteAdditionalInformation.Count());
        }

        [Fact]
        public void CanChangeChemicalCompositionType()
        {
            notification.SetWasteType(WasteType.CreateWoodWasteType("some description", GetWoodWasteTypeCollection()));

            notification.SetWasteType(WasteType.CreateSrfWasteType(GetWasteAdditionalInformationCollection()));

            Assert.Equal(ChemicalComposition.SRF, notification.WasteType.ChemicalCompositionType);
        }
        
        [Fact]
        public void CanChangeChemicalCompositionTypeMultipleTimes()
        {
            notification.SetWasteType(WasteType.CreateWoodWasteType("some description", GetWoodWasteTypeCollection()));

            notification.SetWasteType(WasteType.CreateSrfWasteType(GetWasteAdditionalInformationCollection()));

            notification.SetWasteType(WasteType.CreateWoodWasteType("some description", GetWoodWasteTypeCollection()));

            Assert.Equal(ChemicalComposition.Wood, notification.WasteType.ChemicalCompositionType);
        }

        [Fact]
        public void CanEditChemicalCompositions()
        {
            notification.SetWasteType(WasteType.CreateSrfWasteType(GetWasteAdditionalInformationCollection()));

            List<WasteAdditionalInformation> editedWasteCompositions = new List<WasteAdditionalInformation>();
            editedWasteCompositions.Add(WasteAdditionalInformation.CreateWasteAdditionalInformation("First Constituent", 50, 100, WasteInformationType.AshContent));
            editedWasteCompositions.Add(WasteAdditionalInformation.CreateWasteAdditionalInformation("Second Constituent", 20, 100, WasteInformationType.Chlorine));
            editedWasteCompositions.Add(WasteAdditionalInformation.CreateWasteAdditionalInformation("Third Constituent", 30, 100, WasteInformationType.NetCalorificValue));
            editedWasteCompositions.Add(WasteAdditionalInformation.CreateWasteAdditionalInformation("Fourth Constituent", 40, 100, WasteInformationType.MoistureContent));

            notification.SetWasteType(WasteType.CreateSrfWasteType(editedWasteCompositions));

            Assert.Collection(notification.WasteType.WasteAdditionalInformation,
                item => Assert.Equal(50, notification.WasteType.WasteAdditionalInformation.Single(p => p.Constituent == "First Constituent").MinConcentration),
                item => Assert.Equal(20, notification.WasteType.WasteAdditionalInformation.Single(p => p.Constituent == "Second Constituent").MinConcentration),
                item => Assert.Equal(30, notification.WasteType.WasteAdditionalInformation.Single(p => p.Constituent == "Third Constituent").MinConcentration),
                item => Assert.Equal(40, notification.WasteType.WasteAdditionalInformation.Single(p => p.Constituent == "Fourth Constituent").MinConcentration));
        }

        [Fact]
        public void CanAddWasteAdditionalInformation()
        {
            notification.SetWasteType(WasteType.CreateSrfWasteType(GetWasteAdditionalInformationCollection()));

            notification.SetWasteAdditionalInformation(GetWasteAdditionalInformationCollection());

            Assert.Equal(5, notification.WasteType.WasteAdditionalInformation.Count());
        }

        [Fact]
        public void CanEditWasteAdditionalInformation()
        {
            notification.SetWasteType(WasteType.CreateSrfWasteType(GetWasteAdditionalInformationCollection()));

            notification.SetWasteAdditionalInformation(GetWasteAdditionalInformationCollection());

            List<WasteAdditionalInformation> editedWasteAdditionalInformation = new List<WasteAdditionalInformation>();
            editedWasteAdditionalInformation.Add(WasteAdditionalInformation.CreateWasteAdditionalInformation("First Constituent", 50, 100, WasteInformationType.AshContent));
            editedWasteAdditionalInformation.Add(WasteAdditionalInformation.CreateWasteAdditionalInformation("Second Constituent", 50, 100, WasteInformationType.Chlorine));
            editedWasteAdditionalInformation.Add(WasteAdditionalInformation.CreateWasteAdditionalInformation("Third Constituent", 50, 100, WasteInformationType.Energy));
            editedWasteAdditionalInformation.Add(WasteAdditionalInformation.CreateWasteAdditionalInformation("Fourth Constituent", 50, 100, WasteInformationType.HeavyMetals));
            editedWasteAdditionalInformation.Add(WasteAdditionalInformation.CreateWasteAdditionalInformation("Fifth Constituent", 50, 100, WasteInformationType.MoistureContent));
            editedWasteAdditionalInformation.Add(WasteAdditionalInformation.CreateWasteAdditionalInformation("Sixth Constituent", 50, 100, WasteInformationType.NetCalorificValue));

            notification.SetWasteAdditionalInformation(editedWasteAdditionalInformation);

            Assert.Collection(notification.WasteType.WasteAdditionalInformation,
                item => Assert.Equal(50, notification.WasteType.WasteAdditionalInformation.Single(p => p.Constituent == "First Constituent").MinConcentration),
                item => Assert.Equal(50, notification.WasteType.WasteAdditionalInformation.Single(p => p.Constituent == "Second Constituent").MinConcentration),
                item => Assert.Equal(50, notification.WasteType.WasteAdditionalInformation.Single(p => p.Constituent == "Third Constituent").MinConcentration),
                item => Assert.Equal(50, notification.WasteType.WasteAdditionalInformation.Single(p => p.Constituent == "Fourth Constituent").MinConcentration),
                item => Assert.Equal(50, notification.WasteType.WasteAdditionalInformation.Single(p => p.Constituent == "Fifth Constituent").MinConcentration),
                item => Assert.Equal(50, notification.WasteType.WasteAdditionalInformation.Single(p => p.Constituent == "Sixth Constituent").MinConcentration));
        }

        [Fact]
        public void CantAddWasteTypeForOtherWithoutOtherName()
        {
            Action addOtherWasteTypeWithoutName = () => notification.SetWasteType(WasteType.CreateOtherWasteType(null));
            Assert.Throws<ArgumentNullException>(addOtherWasteTypeWithoutName);
        }

        [Fact]
        public void CanAddWasteTypeForRdfWithoutComposition()
        {
            notification.SetWasteType(WasteType.CreateRdfWasteType(null));
            Assert.NotNull(notification.WasteType);
        }

        private List<WasteComposition> GetWasteTypeCollection()
        {
            List<WasteComposition> wasteCompositions = new List<WasteComposition>();
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("First Constituent", 1, 100, ChemicalCompositionCategory.Metals));
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("Second Constituent", 2, 100, ChemicalCompositionCategory.Wood));
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("Third Constituent", 3, 100, ChemicalCompositionCategory.Food));
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("Fourth Constituent", 4, 100, ChemicalCompositionCategory.Textiles));
            wasteCompositions.Add(WasteComposition.CreateWasteComposition("Fifth Constituent", 5, 100, ChemicalCompositionCategory.Plastics));
            return wasteCompositions;
        }

        private List<WasteAdditionalInformation> GetWoodWasteTypeCollection()
        {
            List<WasteAdditionalInformation> wasteAdditionalInformation = new List<WasteAdditionalInformation>();
            wasteAdditionalInformation.Add(WasteAdditionalInformation.CreateWasteAdditionalInformation("First Constituent", 5, 10, WasteInformationType.AshContent));
            wasteAdditionalInformation.Add(WasteAdditionalInformation.CreateWasteAdditionalInformation("Second Constituent", 5, 10, WasteInformationType.Chlorine));
            wasteAdditionalInformation.Add(WasteAdditionalInformation.CreateWasteAdditionalInformation("Third Constituent", 5, 10, WasteInformationType.HeavyMetals));
            wasteAdditionalInformation.Add(WasteAdditionalInformation.CreateWasteAdditionalInformation("Fourth Constituent", 5, 10, WasteInformationType.MoistureContent));
            wasteAdditionalInformation.Add(WasteAdditionalInformation.CreateWasteAdditionalInformation("Fifth Constituent", 5, 10, WasteInformationType.NetCalorificValue));
            return wasteAdditionalInformation;
        }

        private List<WasteAdditionalInformation> GetWasteAdditionalInformationCollection()
        {
            List<WasteAdditionalInformation> wasteAdditionalInformation = new List<WasteAdditionalInformation>();
            wasteAdditionalInformation.Add(WasteAdditionalInformation.CreateWasteAdditionalInformation("First Constituent", 5, 10, WasteInformationType.AshContent));
            wasteAdditionalInformation.Add(WasteAdditionalInformation.CreateWasteAdditionalInformation("Second Constituent", 5, 10, WasteInformationType.Chlorine));
            wasteAdditionalInformation.Add(WasteAdditionalInformation.CreateWasteAdditionalInformation("Third  Constituent", 5, 10, WasteInformationType.HeavyMetals));
            wasteAdditionalInformation.Add(WasteAdditionalInformation.CreateWasteAdditionalInformation("Fourth Constituent", 5, 10, WasteInformationType.MoistureContent));
            wasteAdditionalInformation.Add(WasteAdditionalInformation.CreateWasteAdditionalInformation("Fifth Constituent", 5, 10, WasteInformationType.NetCalorificValue));
            return wasteAdditionalInformation;
        }
    }
}
