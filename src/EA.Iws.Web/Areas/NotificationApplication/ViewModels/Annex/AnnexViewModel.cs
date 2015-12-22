namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Annex
{
    using System;
    using System.Web;
    using Core.Annexes;

    public class AnnexViewModel
    {
        public Guid NotificationId { get; set; }

        public AnnexStatus TechnologyEmployedStatus { get; set; }

        public AnnexStatus ProcessOfGenerationStatus { get; set; }

        public AnnexStatus WasteCompositionStatus { get; set; }

        public HttpPostedFileBase TechnologyEmployed { get; set; }
        
        public HttpPostedFileBase ProcessOfGeneration { get; set; }

        public HttpPostedFileBase Composition { get; set; }

        public bool AreAnnexesRequired
        {
            get { return TechnologyEmployedStatus.IsRequired || ProcessOfGenerationStatus.IsRequired || WasteCompositionStatus.IsRequired; }
        }

        public AnnexViewModel()
        {
            TechnologyEmployedStatus = new AnnexStatus();
            ProcessOfGenerationStatus = new AnnexStatus();
            WasteCompositionStatus = new AnnexStatus();
        }
    }
}