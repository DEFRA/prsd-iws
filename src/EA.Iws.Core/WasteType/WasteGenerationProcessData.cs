namespace EA.Iws.Core.WasteType
{
    using System;

    public class WasteGenerationProcessData
    {
        public string Process { get; set; }

        public Guid NotificationId { get; set; }

        public bool IsDocumentAttached { get; set; }
    }
}