namespace EA.Iws.Core.WasteType
{
    using System;
    using System.Collections.Generic;

    public class WasteTypeData
    {
        public Guid Id { get; set; }

        public Guid NotificationId { get; set; }

        public ChemicalComposition ChemicalCompositionType { get; set; }

        public string ChemicalCompositionName { get; set; }

        public string ChemicalCompositionDescription { get; set; }

        public List<WasteCompositionData> WasteCompositionData { get; set; }

        public List<WoodInformationData> WasteAdditionalInformation { get; set; }
        
        public string OtherWasteTypeDescription { get; set; }

        public string EnergyInformation { get; set; }

        public string FurtherInformation { get; set; }

        public string WoodTypeDescription { get; set; }

        public bool HasAnnex { get; set; }
    }
}
