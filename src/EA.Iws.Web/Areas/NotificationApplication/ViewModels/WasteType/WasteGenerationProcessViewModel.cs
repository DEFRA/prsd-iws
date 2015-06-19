namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteType
{
    using System;
    using Prsd.Core.Validation;

    public class WasteGenerationProcessViewModel
    {
        public Guid NotificationId { get; set; }

        [RequiredIf("IsDocumentAttached", false, "Please enter a description or check the box")]
        public string ProcessDescription { get; set; }

        [RequiredIf("ProcessDescription", "", "Please enter a description or check the box")]
        public bool IsDocumentAttached { get; set; }
    }
}