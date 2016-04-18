namespace EA.Iws.Requests.WasteCodes
{
    using System.Collections.Generic;
    using Core.WasteCodes;

    public class WasteCodeDataAndNotificationData
    {
        public Dictionary<CodeType, WasteCodeData[]> LookupWasteCodeData { get; set; }

        public Dictionary<CodeType, WasteCodeData[]> NotificationWasteCodeData { get; set; }

        public IList<CodeType> NotApplicableCodes { get; set; }
    }
}