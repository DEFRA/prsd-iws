namespace EA.Iws.Requests.Notification
{
    using System;
    using System.Collections.Generic;
    using Core.WasteType;

    public class ClassifyYourWasteInfo
    {
        public Guid NotificationId { get; set; }
        public bool IsChemicalCompositionCompleted { get; set; }
        public bool IsProcessOfGenerationCompleted { get; set; }
        public bool ArePhysicalCharacteristicsCompleted { get; set; }
        public WasteTypeData ChemicalComposition { get; set; }
        public WasteGenerationProcessData ProcessOfGeneration { get; set; }
        public List<string> PhysicalCharacteristics { get; set; }
    }
}
