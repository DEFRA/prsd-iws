namespace EA.Iws.Web.Areas.AdminImportAssessment.ViewModels.MarkAsInterim
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Core.ImportNotification;
    using Core.ImportNotificationAssessment;
    using Core.Shared;

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
            NotificationType = data.NotificationType;
        }

        public Guid NotificationId { get; set; }

        [Required]
        public bool? IsInterim { get; set; }

        public ImportNotificationStatus NotificationStatus { get; set; }

        public NotificationType NotificationType { get; set; }

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
                       NotificationStatus == ImportNotificationStatus.DecisionRequiredBy;
            }
        }

        public bool InterimStatusIsUpdateable
        {
            get
            {
                return HasAcceptableStatus && NotificationType == NotificationType.Disposal;
            }
        }
    }
}