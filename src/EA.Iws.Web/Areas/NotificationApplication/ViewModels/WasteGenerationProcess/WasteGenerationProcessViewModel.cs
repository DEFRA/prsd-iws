namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteGenerationProcess
{
    using System;
    using Core.WasteType;
    using Prsd.Core.Validation;
    using Requests.WasteType;

    public class WasteGenerationProcessViewModel
    {
        public WasteGenerationProcessViewModel()
        {
        }

        public WasteGenerationProcessViewModel(WasteGenerationProcessData wasteGenerationProcessData)
        {
            NotificationId = wasteGenerationProcessData.NotificationId;
            ProcessDescription = wasteGenerationProcessData.Process;
            IsDocumentAttached = wasteGenerationProcessData.IsDocumentAttached;
        }

        public SetWasteGenerationProcess ToRequest()
        {
            return new SetWasteGenerationProcess(ProcessDescription, NotificationId, IsDocumentAttached);
        }

        public Guid NotificationId { get; set; }

        [RequiredIf("IsDocumentAttached", false, ErrorMessage = "Please enter a description or check the box")]
        public string ProcessDescription { get; set; }

        [RequiredIf("ProcessDescription", "", ErrorMessage = "Please enter a description or check the box")]
        public bool IsDocumentAttached { get; set; }
    }
}