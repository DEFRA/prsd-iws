namespace EA.Iws.Web.ViewModels.WasteType
{
    using System;
    using Prsd.Core.Validation;
    public class WasteGenerationProcessViewModel
    {
        public Guid NotificationId { get; set; }

        public string ProcessDescription { get; set; }

        public bool IsDocumentAttached { get; set; }
    }
}