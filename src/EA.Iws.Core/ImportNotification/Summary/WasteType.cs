namespace EA.Iws.Core.ImportNotification.Summary
{
    public class WasteType
    {
        public string Name { get; set; }

        public WasteCodeSelection BaselCode { get; set; }

        public WasteCodeSelection EwcCodes { get; set; }

        public WasteCodeSelection YCodes { get; set; }

        public WasteCodeSelection HCodes { get; set; }

        public WasteCodeSelection UnClasses { get; set; }
    }
}
