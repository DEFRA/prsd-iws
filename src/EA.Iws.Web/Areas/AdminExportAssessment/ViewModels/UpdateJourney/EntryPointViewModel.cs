namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels.UpdateJourney
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Core.StateOfImport;
    using Core.TransportRoute;
    using EA.Iws.Core.Shared;

    public class EntryPointViewModel
    {
        public EntryPointViewModel(StateOfImportData stateOfImport, IList<EntryOrExitPointData> entryPoints, Guid notificationId)
        {
            CompetentAuthority = stateOfImport.CompetentAuthority.Name;
            EntryPoint = stateOfImport.EntryPoint.Name;
            EntryPoints = new SelectList(entryPoints, "Id", "Name");
            AdditionalCharge = new AdditionalChargeData()
            {
                NotificationId = notificationId
            };
        }

        public EntryPointViewModel()
        {
        }

        public string CompetentAuthority { get; set; }

        public string EntryPoint { get; set; }

        public SelectList EntryPoints { get; set; }

        [Display(ResourceType = typeof(UpdateJourneyResources), Name = "EntryPoint")]
        [Required(ErrorMessageResourceType = typeof(UpdateJourneyResources), ErrorMessageResourceName = "EntryPointRequired")]
        public Guid? SelectedEntryPoint { get; set; }

        public AdditionalChargeData AdditionalCharge { get; set; }
    }
}