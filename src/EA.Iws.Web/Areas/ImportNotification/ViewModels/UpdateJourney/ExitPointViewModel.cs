namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.UpdateJourney
{
    using Core.StateOfExport;
    using Core.TransportRoute;
    using EA.Iws.Core.ImportNotificationAssessment;
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.NotificationAssessment;
    using EA.Iws.Core.Shared;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class ExitPointViewModel
    {
        public ExitPointViewModel(StateOfExportData stateOfExport, IList<EntryOrExitPointData> entryPoints,
                                  Guid notificationId, UKCompetentAuthority authority, ImportNotificationStatus notificationStatus)
        {
            CompetentAuthority = stateOfExport.CompetentAuthority.Name;
            ExitPoint = stateOfExport.ExitPoint.Name;
            ExitPoints = new SelectList(entryPoints, "Id", "Name");
            AdditionalCharge = new AdditionalChargeData()
            {
                NotificationId = notificationId
            };
            NotificationCompetentAuthority = authority;
            NotificationStatus = notificationStatus;
            ShowAdditionalCharge = (authority == UKCompetentAuthority.England || authority == UKCompetentAuthority.Scotland)
                && (notificationStatus == ImportNotificationStatus.Consented || notificationStatus == ImportNotificationStatus.DecisionRequiredBy);
        }

        public ExitPointViewModel()
        {
        }

        public string CompetentAuthority { get; set; }

        public string ExitPoint { get; set; }

        public SelectList ExitPoints { get; set; }

        [Display(ResourceType = typeof(UpdateJourneyResources), Name = "ExitPoint")]
        [Required(ErrorMessageResourceType = typeof(UpdateJourneyResources),
            ErrorMessageResourceName = "ExitPointRequired")]
        public Guid? SelectedExitPoint { get; set; }

        public AdditionalChargeData AdditionalCharge { get; set; }

        public UKCompetentAuthority NotificationCompetentAuthority { get; set; }

        public bool ShowAdditionalCharge { get; set; }

        public ImportNotificationStatus NotificationStatus { get; set; }
    }
}