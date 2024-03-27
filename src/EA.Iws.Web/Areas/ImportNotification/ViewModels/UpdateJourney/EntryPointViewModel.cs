namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.UpdateJourney
{
    using Core.StateOfImport;
    using Core.TransportRoute;
    using EA.Iws.Core.ImportNotificationAssessment;
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.Shared;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class EntryPointViewModel
    {
        public EntryPointViewModel(StateOfImportData stateOfImport, IList<EntryOrExitPointData> entryPoints,
                                   Guid notificationId, UKCompetentAuthority authority, ImportNotificationStatus notificationStatus)
        {
            CompetentAuthority = stateOfImport.CompetentAuthority.Name;
            EntryPoint = stateOfImport.EntryPoint.Name;
            EntryPoints = new SelectList(entryPoints, "Id", "Name");
            AdditionalCharge = new AdditionalChargeData()
            {
                NotificationId = notificationId
            };
            NotificationCompetentAuthority = authority;
            NotificationStatus = notificationStatus;
            ShowAdditionalCharge = (authority == UKCompetentAuthority.England || authority == UKCompetentAuthority.Scotland)
                && (notificationStatus == ImportNotificationStatus.Consented || notificationStatus == ImportNotificationStatus.DecisionRequiredBy);
        }

        public EntryPointViewModel()
        {
        }

        public string CompetentAuthority { get; set; }

        public string EntryPoint { get; set; }

        public SelectList EntryPoints { get; set; }

        [Display(ResourceType = typeof(UpdateJourneyResources), Name = "EntryPoint")]
        [Required(ErrorMessageResourceType = typeof(UpdateJourneyResources),
            ErrorMessageResourceName = "EntryPointRequired")]
        public Guid? SelectedEntryPoint { get; set; }

        public AdditionalChargeData AdditionalCharge { get; set; }

        public UKCompetentAuthority NotificationCompetentAuthority { get; set; }

        public bool ShowAdditionalCharge { get; set; }

        public ImportNotificationStatus NotificationStatus { get; set; }
    }
}