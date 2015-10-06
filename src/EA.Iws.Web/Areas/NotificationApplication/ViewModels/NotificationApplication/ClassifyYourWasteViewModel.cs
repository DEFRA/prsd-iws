namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using Core.Notification;
    using Core.WasteType;
    using Requests.Notification.Overview;

    public class ClassifyYourWasteViewModel
    {
        public Guid NotificationId { get; set; }
        public bool IsChemicalCompositionCompleted { get; set; }
        public bool IsProcessOfGenerationCompleted { get; set; }
        public bool ArePhysicalCharacteristicsCompleted { get; set; }
        public WasteTypeData ChemicalComposition { get; set; }
        public string ProcessOfGeneration { get; set; }
        public List<string> PhysicalCharacteristics { get; set; }

        public ClassifyYourWasteViewModel()
        {
        }

        public ClassifyYourWasteViewModel(WasteClassificationOverview classifyYourWasteInfo, NotificationApplicationCompletionProgress progress)
        {
            NotificationId = classifyYourWasteInfo.NotificationId;
            IsChemicalCompositionCompleted = progress.HasWasteType;
            IsProcessOfGenerationCompleted = progress.HasWasteGenerationProcess;
            ArePhysicalCharacteristicsCompleted = progress.HasPhysicalCharacteristics;
            ChemicalComposition = classifyYourWasteInfo.ChemicalComposition;
            ProcessOfGeneration = classifyYourWasteInfo.ProcessOfGeneration.IsDocumentAttached ? 
                "The details will be provided in a separate document" : classifyYourWasteInfo.ProcessOfGeneration.Process;
            PhysicalCharacteristics = classifyYourWasteInfo.PhysicalCharacteristics;
        }
    }
}