namespace EA.Iws.Requests.WasteType
{
    using System;
    using System.Collections.Generic;

    public class WasteTypeData
    {
        public Guid Id { get; set; }

        public Guid NotificationId { get; set; }

        public ChemicalCompositionType ChemicalCompositionType { get; set; }

        public string ChemicalCompositionName { get; set; }

        public string ChemicalCompositionDescription { get; set; }

        public List<WasteCompositionData> WasteCompositionData { get; set; }
    }
}
