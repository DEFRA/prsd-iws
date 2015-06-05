namespace EA.Iws.Domain.Notification
{
    using Prsd.Core.Domain;

    public class WasteComposition : Entity
    {
        protected WasteComposition()
        {
        }

        internal WasteComposition(string constituent, decimal minConcentration, decimal maxConcentration)
        {
            Constituent = constituent;
            MinConcentration = minConcentration;
            MaxConcentration = maxConcentration;
        }

        public string Constituent { get; internal set; }

        public decimal MinConcentration { get; internal set; }

        public decimal MaxConcentration { get; internal set; }
    }
}