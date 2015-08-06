namespace EA.Iws.Requests.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.Shared;
    using Core.WasteCodes;
    using Core.WasteType;

    public class ClassifyYourWasteInfo
    {
        public Guid NotificationId { get; set; }
        public NotificationApplicationCompletionProgress Progress { get; set; }
        public WasteTypeData ChemicalComposition { get; set; }
        public WasteGenerationProcessData ProcessOfGeneration { get; set; }
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
    }
}
