namespace EA.Iws.Domain.NotificationApplication.Annexes
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class AnnexCollection : Entity
    {
        public Guid NotificationId { get; private set; }

        public ProcessOfGenerationAnnex ProcessOfGeneration { get; private set; }

        public WasteCompositionAnnex WasteComposition { get; private set; }

        public TechnologyEmployedAnnex TechnologyEmployed { get; private set; }

        protected AnnexCollection()
        {
        }

        public AnnexCollection(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public void SetProcessOfGenerationAnnex(ProcessOfGenerationAnnex processOfGenerationAnnex)
        {
            Guard.ArgumentNotNull(() => processOfGenerationAnnex, processOfGenerationAnnex);

            ProcessOfGeneration = processOfGenerationAnnex;
        }
    }
}
