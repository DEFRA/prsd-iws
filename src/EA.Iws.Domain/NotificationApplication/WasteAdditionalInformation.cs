namespace EA.Iws.Domain.NotificationApplication
{
    using Core.WasteType;
    using Prsd.Core.Domain;

    public class WasteAdditionalInformation : Entity
    {
        protected WasteAdditionalInformation()
        {
        }

        private WasteAdditionalInformation(string constituent, decimal minConcentration, decimal maxConcentration, WasteInformationType wasteInformationType)
        {
            Constituent = constituent;
            MinConcentration = minConcentration;
            MaxConcentration = maxConcentration;
            WasteInformationType = wasteInformationType;
        }

        public string Constituent { get; private set; }

        public decimal MinConcentration { get; private set; }

        public decimal MaxConcentration { get; private set; }

        public WasteInformationType WasteInformationType { get; private set; }

        public static WasteAdditionalInformation CreateWasteAdditionalInformation(string constituent, decimal minConcentration, decimal maxConcentration, WasteInformationType wasteInformationType)
        {
            return new WasteAdditionalInformation(constituent, minConcentration, maxConcentration, wasteInformationType);
        }
    }
}