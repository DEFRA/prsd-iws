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

        public virtual WasteCode WasteCode { get; private set; }
    }
}