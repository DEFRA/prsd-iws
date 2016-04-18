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
            ProcessOfGeneration = new ProcessOfGenerationAnnex();
            WasteComposition = new WasteCompositionAnnex();
            TechnologyEmployed = new TechnologyEmployedAnnex();
        }

        public void SetProcessOfGenerationAnnex(ProcessOfGenerationAnnex processOfGenerationAnnex)
        {
            Guard.ArgumentNotNull(() => processOfGenerationAnnex, processOfGenerationAnnex);

            ProcessOfGeneration = processOfGenerationAnnex;
        }

        public void SetWasteCompositionAnnex(WasteCompositionAnnex wasteCompositionAnnex)
        {
            Guard.ArgumentNotNull(() => wasteCompositionAnnex, wasteCompositionAnnex);

            WasteComposition = wasteCompositionAnnex;
        }

        public void SetTechnologyEmployedAnnex(TechnologyEmployedAnnex technologyEmployedAnnex)
        {
            Guard.ArgumentNotNull(() => technologyEmployedAnnex, technologyEmployedAnnex);

            TechnologyEmployed = technologyEmployedAnnex;
        }

        public void RemoveAnnex(Guid fileId)
        {
            if (ProcessOfGeneration.FileId == fileId)
            {
                ProcessOfGeneration = new ProcessOfGenerationAnnex();
            }
            else if (TechnologyEmployed.FileId == fileId)
            {
                TechnologyEmployed = new TechnologyEmployedAnnex();
            }
            else if (WasteComposition.FileId == fileId)
            {
                WasteComposition = new WasteCompositionAnnex();
            }

            RaiseEvent(new DeleteAnnexEvent(fileId));
        }
    }
}
