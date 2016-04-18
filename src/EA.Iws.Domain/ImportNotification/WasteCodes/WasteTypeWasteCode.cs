namespace EA.Iws.Domain.ImportNotification.WasteCodes
{
    using System;
    using Prsd.Core.Domain;

    public class WasteTypeWasteCode : Entity
    {
        public Guid WasteCodeId { get; private set; }

        protected WasteTypeWasteCode()
        {
        }

        public WasteTypeWasteCode(Guid wasteCodeId)
        {
            WasteCodeId = wasteCodeId;
        }
    }
}