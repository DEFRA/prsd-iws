namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.WasteCodes;

    public class WasteCodesOverviewInfo
    {
        public Guid NotificationId { get; set; }
        public bool IsBaselOecdCodeCompleted { get; set; }
        public bool AreEwcCodesCompleted { get; set; }
        public bool AreYCodesCompleted { get; set; }
        public bool AreHCodesCompleted { get; set; }
        public bool AreUnClassesCompleted { get; set; }
        public bool AreUnNumbersCompleted { get; set; }
        public bool AreOtherCodesCompleted { get; set; }
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
