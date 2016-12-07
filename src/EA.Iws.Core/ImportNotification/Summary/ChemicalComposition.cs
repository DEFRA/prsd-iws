namespace EA.Iws.Core.ImportNotification.Summary
{
    public class ChemicalComposition
    {
        public Core.WasteType.ChemicalComposition? Composition { get; set; }

        public bool IsEmpty()
        {
            return !Composition.HasValue;
        }
    }
}