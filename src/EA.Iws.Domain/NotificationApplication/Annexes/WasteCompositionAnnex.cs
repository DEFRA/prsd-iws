namespace EA.Iws.Domain.NotificationApplication.Annexes
{
    using System;

    public class WasteCompositionAnnex : Annex
    {
        protected WasteCompositionAnnex()
        {
        }

        public WasteCompositionAnnex(Guid fileId) : base(fileId)
        {
        }
    }
}