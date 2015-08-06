namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Core.Notification;
    using Core.Shared;
    using Core.WasteCodes;
    using Core.WasteType;
    using Requests.Notification;

    public class ClassifyYourWasteViewModel
    {
        public Guid NotificationId { get; set; }
        public NotificationApplicationCompletionProgress Progress { get; set; }
        public WasteTypeData ChemicalComposition { get; set; }
        public string ProcessOfGeneration { get; set; }
        public List<string> PhysicalCharacteristics { get; set; }
        public WasteCodeData[] BaselOecdCode { get; set; }
        public WasteCodeData[] EwcCodes { get; set; }
        public WasteCodeData[] NationExportCode { get; set; }
        public WasteCodeData[] NationImportCode { get; set; }
        public WasteCodeData[] OtherCodes { get; set; }
        public WasteCodeData[] YCodes { get; set; }
        public WasteCodeData[] HCodes { get; set; }
        public WasteCodeData[] UnClass { get; set; }
        public WasteCodeData[] UnNumber { get; set; }
        public WasteCodeData[] CustomCodes { get; set; }

        public ClassifyYourWasteViewModel()
        {
        }

        public ClassifyYourWasteViewModel(ClassifyYourWasteInfo classifyYourWasteInfo)
        {
            NotificationId = classifyYourWasteInfo.NotificationId;
            Progress = classifyYourWasteInfo.Progress;
            ChemicalComposition = classifyYourWasteInfo.ChemicalComposition;
            ProcessOfGeneration = classifyYourWasteInfo.ProcessOfGeneration.IsDocumentAttached ? 
                "The details will be provided in a separate document" : classifyYourWasteInfo.ProcessOfGeneration.Process;
            PhysicalCharacteristics = classifyYourWasteInfo.PhysicalCharacteristics;
            BaselOecdCode = classifyYourWasteInfo.BaselOecdCode;
            EwcCodes = classifyYourWasteInfo.EwcCodes;
            NationExportCode = classifyYourWasteInfo.NationExportCode;
            NationImportCode = classifyYourWasteInfo.NationImportCode;
            OtherCodes = classifyYourWasteInfo.OtherCodes;
            YCodes = classifyYourWasteInfo.YCodes;
            HCodes = classifyYourWasteInfo.HCodes;
            UnClass = classifyYourWasteInfo.UnClass;
            UnNumber = classifyYourWasteInfo.UnNumber;
            CustomCodes = classifyYourWasteInfo.CustomCodes;
        }
    }
}