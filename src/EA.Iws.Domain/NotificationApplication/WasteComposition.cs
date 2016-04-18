namespace EA.Iws.Domain.NotificationApplication
{
    using Core.WasteType;
    using Prsd.Core.Domain;

    public class WasteComposition : Entity
    {
        protected WasteComposition()
        {
        }

        internal WasteComposition(string constituent, decimal minConcentration, decimal maxConcentration, ChemicalCompositionCategory chemicalCompositionType)
        {
            Constituent = constituent;
            MinConcentration = minConcentration;
            MaxConcentration = maxConcentration;
            ChemicalCompositionType = chemicalCompositionType;
        }

        public string Constituent { get; internal set; }

        public decimal MinConcentration { get; internal set; }

        public decimal MaxConcentration { get; internal set; }

        public ChemicalCompositionCategory ChemicalCompositionType { get; internal set; }

        public static WasteComposition CreateWasteComposition(string constituent, decimal minConcentration, decimal maxConcentration, ChemicalCompositionCategory chemicalCompositionType)
        {
            return new WasteComposition(constituent, minConcentration, maxConcentration, chemicalCompositionType);
        }
    }
}