namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteGenerationProcess
{
    using System;
    using Core.WasteType;
    using Prsd.Core.Validation;
    using Requests.WasteType;
    using Views.WasteGenerationProcess;

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

        [RequiredIf("IsDocumentAttached", false, ErrorMessageResourceName = "AnyOneRequired", ErrorMessageResourceType = typeof(WasteGenerationProcessResources))]
        public string ProcessDescription { get; set; }

        [RequiredIf("ProcessDescription", "", ErrorMessageResourceName = "AnyOneRequired", ErrorMessageResourceType = typeof(WasteGenerationProcessResources))]
        public bool IsDocumentAttached { get; set; }
    }
}