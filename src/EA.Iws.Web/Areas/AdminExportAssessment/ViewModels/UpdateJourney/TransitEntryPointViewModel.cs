namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.UpdateJourney
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Core.TransitState;
    using Core.TransportRoute;

    public class TransitEntryPointViewModel
    {
        public string CompetentAuthority { get; set; }

        public string EntryPoint { get; set; }

        public SelectList EntryPoints { get; set; }

        [Display(ResourceType = typeof(UpdateJourneyResources), Name = "EntryPoint")]
        [Required(ErrorMessageResourceType = typeof(UpdateJourneyResources), ErrorMessageResourceName = "EntryPointRequired")]
        public Guid? SelectedEntryPoint { get; set; }

        public TransitEntryPointViewModel(TransitStateData transitState, IEnumerable<EntryOrExitPointData> entryOrExitPoints)
        {
            CompetentAuthority = string.Format("{0} - {1}", transitState.CompetentAuthority.Code,
                transitState.CompetentAuthority.Name);
            EntryPoint = transitState.EntryPoint.Name;
            EntryPoints = new SelectList(entryOrExitPoints, "Id", "Name");
        }
    }
}