namespace EA.Iws.Domain.ImportNotification.WasteCodes
{
    using System;
    using Prsd.Core.Domain;

    public class WasteCode : Entity
    {
        public Guid WasteCodeId { get; private set; }

        protected WasteCode()
        {
        }

        public WasteCode(Guid wasteCodeId)
        {
            WasteCodeId = wasteCodeId;
        }
    }
}