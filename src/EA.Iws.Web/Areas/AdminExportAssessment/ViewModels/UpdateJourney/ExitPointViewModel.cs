namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.UpdateJourney
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Core.StateOfExport;
    using Core.TransportRoute;
    using EA.Iws.Core.Shared;

    public class ExitPointViewModel
    {
        public ExitPointViewModel(StateOfExportData stateOfExport, IList<EntryOrExitPointData> entryPoints, Guid notificationId)
        {
            CompetentAuthority = stateOfExport.CompetentAuthority.Name;
            ExitPoint = stateOfExport.ExitPoint.Name;
            ExitPoints = new SelectList(entryPoints, "Id", "Name");
            AdditionalCharge = new AdditionalChargeData()
            {
                NotificationId = notificationId
            };
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

        public AdditionalChargeData AdditionalCharge { get; set; }
    }
}