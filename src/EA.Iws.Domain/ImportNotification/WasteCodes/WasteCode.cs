namespace EA.Iws.Domain.ImportNotification.WasteCodes
{
    using System;
    using Core.WasteCodes;
    using Prsd.Core.Domain;

    public class WasteCode : Entity
    {
        public Guid WasteCodeId { get; private set; }

        public CodeType Type { get; private set; }

        protected WasteCode()
        {
        }

        public WasteCode(Guid wasteCodeId, CodeType type)
        {
            WasteCodeId = wasteCodeId;
            Type = type;
        }
    }
}