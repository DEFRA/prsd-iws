namespace EA.Iws.Domain.ImportNotification
{
    using EA.Prsd.Core.Domain;

    public class WasteComponentCode : Entity
    {
        protected WasteComponentCode()
        {
        }

        public WasteComponentCode(WasteComponentCode wasteComponentCode)
        {
            ComponentCode = wasteComponentCode;
        }

        public WasteComponentCode ComponentCode { get; private set; }
    }
}
