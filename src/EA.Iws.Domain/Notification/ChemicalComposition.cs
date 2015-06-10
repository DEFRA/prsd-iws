namespace EA.Iws.Domain.Notification
{
    using Prsd.Core.Domain;

    public class ChemicalComposition : Enumeration
    {
        public static readonly ChemicalComposition NotSet = new ChemicalComposition(0, "Not Set");
        public static readonly ChemicalComposition RDF = new ChemicalComposition(1, "RDF");
        public static readonly ChemicalComposition SRF = new ChemicalComposition(2, "SRF");
        public static readonly ChemicalComposition Wood = new ChemicalComposition(3, "Wood");
        public static readonly ChemicalComposition Other = new ChemicalComposition(4, "Other");

        protected ChemicalComposition()
        {
        }

        private ChemicalComposition(int value, string displayName)
            : base(value, displayName)
        {
        }
    }
}