namespace EA.Iws.Domain.NotificationApplication.Annexes
{
    using System;

    public class ProcessOfGenerationAnnex : Annex
    {
        internal ProcessOfGenerationAnnex()
        {
        }

        public ProcessOfGenerationAnnex(Guid fileId) : base(fileId)
        {
        }
    }
}
