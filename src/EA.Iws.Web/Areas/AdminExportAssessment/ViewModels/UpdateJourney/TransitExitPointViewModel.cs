namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.UpdateJourney
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Core.TransitState;
    using Core.TransportRoute;

    public class TransitExitPointViewModel
    {
        public string CompetentAuthority { get; set; }

        public string ExitPoint { get; set; }

        public SelectList ExitPoints { get; set; }

        [Display(ResourceType = typeof(UpdateJourneyResources), Name = "ExitPoint")]
        [Required(ErrorMessageResourceType = typeof(UpdateJourneyResources), ErrorMessageResourceName = "ExitPointRequired")]
        public Guid? SelectedExitPoint { get; set; }

        public TransitExitPointViewModel(TransitStateData transitState, IEnumerable<EntryOrExitPointData> entryOrExitPoints)
        {
            CompetentAuthority = string.Format("{0} - {1}", transitState.CompetentAuthority.Code,
                transitState.CompetentAuthority.Name);
            ExitPoint = transitState.ExitPoint.Name;
            ExitPoints = new SelectList(entryOrExitPoints, "Id", "Name");
        }
    }
}