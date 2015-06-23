namespace EA.Iws.Domain.Notification
{
    using Prsd.Core.Domain;

    public class WasteCodeInfo : Entity
    {
        protected WasteCodeInfo()
        {
        }

        internal WasteCodeInfo(WasteCode wasteCode)
        {
            WasteCode = wasteCode;
        }

        public virtual WasteCode WasteCode { get; internal set; }

        public string OptionalDescription { get; internal set; }

        public string OptionalCode { get; internal set; }
    }
}