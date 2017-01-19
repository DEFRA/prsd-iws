namespace EA.Iws.Web.Areas.AdminImportAssessment.ViewModels.MarkAsInterim
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Core.ImportNotification;
    using Core.ImportNotificationAssessment;

    public class MarkAsInterimViewModel
    {
        public MarkAsInterimViewModel()
        {
        }

        public MarkAsInterimViewModel(NotificationDetails data)
        {
            NotificationId = data.ImportNotificationId;
            IsInterim = data.IsInterim;
            NotificationStatus = data.Status;
        }

        public Guid NotificationId { get; set; }

        [Required]
        public bool? IsInterim { get; set; }

        public ImportNotificationStatus NotificationStatus { get; set; }

        public bool IsAuthorised { get; set; }

        public bool HasAcceptableStatus
        {
            get
            {
                return NotificationStatus == ImportNotificationStatus.NotificationReceived ||
                       NotificationStatus == ImportNotificationStatus.AwaitingPayment ||
                       NotificationStatus == ImportNotificationStatus.AwaitingAssessment ||
                       NotificationStatus == ImportNotificationStatus.InAssessment ||
                       NotificationStatus == ImportNotificationStatus.ReadyToAcknowledge ||
                       NotificationStatus == ImportNotificationStatus.DecisionRequiredBy ||
                       NotificationStatus == ImportNotificationStatus.Consented;
            }
        }
    }
}