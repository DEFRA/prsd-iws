namespace EA.Iws.RequestHandlers.ImportNotification
{
    using Domain.ImportNotification.WasteCodes;
    using EA.Iws.Core.WasteType;

    public class UpdateWasteCodeData
    {
        public string Name { get; set; }

        public BaselOecdCode BaselOecdCode { get; set; }

        public EwcCode EwcCode { get; set; }

        public YCode YCode { get; set; }

        public HCode HCode { get; set; }

        public UnClass UnClass { get; set; }

        public WasteCategoryType? WasteCategoryType { get; set; }
    }
}
