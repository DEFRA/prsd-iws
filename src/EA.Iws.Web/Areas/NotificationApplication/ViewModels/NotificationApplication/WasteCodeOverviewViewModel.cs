namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.NotificationApplication
{
    using System;
    using Core.WasteCodes;
    using Requests.Notification;

    public class WasteCodeOverviewViewModel
    {
        public Guid NotificationId { get; set; }
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
        public bool AreWasteCodesCompleted { get; set; }

        public WasteCodeOverviewViewModel(WasteCodesOverviewInfo classifyYourWasteInfo)
        {
            NotificationId = classifyYourWasteInfo.NotificationId;
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
            AreWasteCodesCompleted = classifyYourWasteInfo.AreWasteCodesCompleted;
        }
    }
}