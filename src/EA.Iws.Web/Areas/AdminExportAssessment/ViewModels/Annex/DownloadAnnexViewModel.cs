namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.Annex
{
    using Core.Annexes;
    using Core.Annexes.ExportNotification;

    public class DownloadAnnexViewModel
    {
        public DownloadAnnexViewModel()
        {
        }

        public DownloadAnnexViewModel(ProvidedAnnexesData data)
        {
            ProcessOfGeneration = data.ProcessOfGeneration;
            TechnologyEmployed = data.TechnologyEmployed;
            WasteComposition = data.WasteComposition;
        }

        public AnnexStatus ProcessOfGeneration { get; set; }

        public AnnexStatus TechnologyEmployed { get; set; }

        public AnnexStatus WasteComposition { get; set; }

        public bool HasAnnexes
        {
            get { return TechnologyEmployed.IsRequired || ProcessOfGeneration.IsRequired || WasteComposition.IsRequired; }
        }
    }
}