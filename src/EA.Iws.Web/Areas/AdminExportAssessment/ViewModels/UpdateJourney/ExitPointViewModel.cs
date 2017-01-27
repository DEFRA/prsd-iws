namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.UpdateJourney
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Core.StateOfExport;
    using Core.TransportRoute;

    public class ExitPointViewModel
    {
        public ExitPointViewModel(StateOfExportData stateOfExport, IList<EntryOrExitPointData> entryPoints)
        {
            CompetentAuthority = stateOfExport.CompetentAuthority.Name;
            ExitPoint = stateOfExport.ExitPoint.Name;
            ExitPoints = new SelectList(entryPoints, "Id", "Name");
        }

        public ExitPointViewModel()
        {
        }

        public string CompetentAuthority { get; set; }

        public string ExitPoint { get; set; }

        public SelectList ExitPoints { get; set; }

        [Display(ResourceType = typeof(UpdateJourneyResources), Name = "ExitPoint")]
        [Required(ErrorMessageResourceType = typeof(UpdateJourneyResources), ErrorMessageResourceName = "ExitPointRequired")]
        public Guid? SelectedExitPoint { get; set; }
    }
}