﻿namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using Core.Notification;
    using Core.Notification.Overview;
    using Core.WasteType;

    public class ClassifyYourWasteViewModel
    {
        public Guid NotificationId { get; set; }
        public bool IsChemicalCompositionCompleted { get; set; }
        public bool IsProcessOfGenerationCompleted { get; set; }
        public bool ArePhysicalCharacteristicsCompleted { get; set; }
        public WasteTypeData ChemicalComposition { get; set; }
        public string ProcessOfGeneration { get; set; }
        public List<string> PhysicalCharacteristics { get; set; }
        public string WasteComponentTypesDescription { get; set; }

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
            ProcessOfGeneration = classifyYourWasteInfo.ProcessOfGeneration.IsDocumentAttached ? "The details will be provided in a separate document" : classifyYourWasteInfo.ProcessOfGeneration.Process;
            PhysicalCharacteristics = classifyYourWasteInfo.PhysicalCharacteristics;
            WasteComponentTypesDescription = string.Join(", ", classifyYourWasteInfo.WasteComponentTypes);
        }

        public string ConstituentTitle(WoodInformationData woodInformationData)
        {
            var name = woodInformationData.Constituent;

            if (woodInformationData.WasteInformationType == WasteInformationType.HeavyMetals)
            {
                name = name + " (" + Views.ChemicalComposition.ParametersResources.Milligrams.Replace(" ", "&nbsp;") + ")";
            }

            if (woodInformationData.WasteInformationType == WasteInformationType.NetCalorificValue)
            {
                name = name + " (" + Views.ChemicalComposition.ParametersResources.Megajoules.Replace(" ", "&nbsp;") + ")";
            }

            return name;
        }

        public string ConstituentUnits(WoodInformationData woodInformationData)
        {
            var units = "%";

            if (woodInformationData.WasteInformationType == WasteInformationType.HeavyMetals || woodInformationData.WasteInformationType == WasteInformationType.NetCalorificValue)
            {
                units = string.Empty;
            }

            return units;
        }
    }
}