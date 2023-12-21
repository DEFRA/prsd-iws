namespace EA.Iws.Core.ImportNotification.Summary
{
    public class WasteCategory
    {
        public Core.WasteType.WasteCategoryType? WasteCategoryType { get; set; }

        public bool IsEmpty()
        {
            return !WasteCategoryType.HasValue;
        }
    }
}
