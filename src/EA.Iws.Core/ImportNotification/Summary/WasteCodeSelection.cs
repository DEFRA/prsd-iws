namespace EA.Iws.Core.ImportNotification.Summary
{
    using System.Collections.Generic;

    public class WasteCodeSelection
    {
        public bool IsNotApplicable { get; set; }

        public IList<WasteCode> WasteCodes { get; set; }

        public WasteCodeSelection()
        {
            WasteCodes = new List<WasteCode>();
        }

        public bool IsEmpty()
        {
            return !IsNotApplicable && WasteCodes.Count == 0;
        }
    }
}
