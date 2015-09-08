namespace EA.Iws.TestHelpers.DomainFakes
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.WasteType;
    using Domain.NotificationApplication;
    using Helpers;

    public class TestableWasteType : WasteType
    {
        public new string EnergyInformation
        {
            get { return base.EnergyInformation; }
            set { energyInformation = value; }
        }

        public new string OptionalInformation
        {
            get { return base.OptionalInformation; }
            set { optionalInformation = value; }
        }

        public new string WoodTypeDescription
        {
            get { return base.WoodTypeDescription; }
            set { woodTypeDescription = value; }
        }

        public new string OtherWasteTypeDescription
        {
            get { return base.OtherWasteTypeDescription; }
            set { otherWasteTypeDescription = value; }
        }

        public new string ChemicalCompositionName
        {
            get { return base.ChemicalCompositionName; }
            set { chemicalCompositionName = value; }
        }

        public new string ChemicalCompositionDescription
        {
            get { return base.ChemicalCompositionDescription; }
            set { chemicalCompositionDescription = value; }
        }

        public new ChemicalComposition ChemicalCompositionType
        {
            get { return base.ChemicalCompositionType; }
            set { ObjectInstantiator<WasteType>.SetProperty(x => x.ChemicalCompositionType, value, this); }
        }

        public new List<WasteAdditionalInformation> WasteAdditionalInformation
        {
            get { return base.WasteAdditionalInformation.ToList(); }
            set { WasteAdditionalInformationCollection = value; }
        }

        public new List<WasteComposition> WasteCompositions
        {
            get { return base.WasteCompositions.ToList(); }
            set { WasteCompositionCollection = value; }
        } 
    }
}
