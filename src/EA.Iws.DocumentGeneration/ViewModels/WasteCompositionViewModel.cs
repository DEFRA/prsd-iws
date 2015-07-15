namespace EA.Iws.DocumentGeneration.ViewModels
{
    using Domain.Notification;

    internal class WasteCompositionViewModel
    {
        public string WasteName { get; private set; }

        public WasteCompositionViewModel(WasteType wasteComposition)
        {
            WasteName = GetWasteName(wasteComposition);
        }

        private string GetWasteName(WasteType wasteComposition)
        {
            if (wasteComposition.ChemicalCompositionType == ChemicalComposition.RDF)
            {
                return "Refuse Derived Fuel (RDF)";
            }
            if (wasteComposition.ChemicalCompositionType == ChemicalComposition.SRF)
            {
                return "Solid Recovered Fuel (SRF)";
            }
            if (wasteComposition.ChemicalCompositionType == ChemicalComposition.Wood)
            {
                return "Wood - " + wasteComposition.ChemicalCompositionDescription;
            }
            if (wasteComposition.ChemicalCompositionType == ChemicalComposition.Other)
            {
                return wasteComposition.ChemicalCompositionName;
            }
            return string.Empty;
        }
    }
}