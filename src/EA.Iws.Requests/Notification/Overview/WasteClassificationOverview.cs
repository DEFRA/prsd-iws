namespace EA.Iws.Requests.Notification.Overview
{
    using System;
    using System.Collections.Generic;
    using Core.WasteType;

    public class WasteClassificationOverview
    {
        public Guid NotificationId { get; set; }
        public WasteTypeData ChemicalComposition { get; set; }
        public WasteGenerationProcessData ProcessOfGeneration { get; set; }
        public List<string> PhysicalCharacteristics { get; set; }
    }
}
