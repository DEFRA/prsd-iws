namespace EA.Iws.Core.ImportNotification.Summary
{
    using System.Collections.Generic;

    public class WasteType
    {
        public string Name { get; set; }

        public WasteCode BaselCode { get; set; }

        public List<WasteCode> EwcCodes { get; set; }

        public List<WasteCode> YCodes { get; set; }

        public List<WasteCode> HCodes { get; set; }

        public List<WasteCode> UnClasses { get; set; }
    }
}
