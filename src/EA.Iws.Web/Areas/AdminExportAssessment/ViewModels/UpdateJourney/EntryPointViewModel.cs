namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.UpdateJourney
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Core.StateOfImport;
    using Core.TransportRoute;

    public class EntryPointViewModel
    {
        public EntryPointViewModel(StateOfImportData stateOfImport, IList<EntryOrExitPointData> entryPoints)
        {
            CompetentAuthority = stateOfImport.CompetentAuthority.Name;
            EntryPoint = stateOfImport.EntryPoint.Name;
            EntryPoints = new SelectList(entryPoints, "Id", "Name");
        }

        public EntryPointViewModel()
        {
        }

        public string CompetentAuthority { get; set; }

        public string EntryPoint { get; set; }

        public SelectList EntryPoints { get; set; }

        [Display(ResourceType = typeof(UpdateJourneyResources), Name = "EntryPoint")]
        [Required]
        public Guid? SelectedEntryPoint { get; set; }
    }
}