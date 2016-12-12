namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Core.NotificationAssessment;
    using Core.Shared;

    public class UpdateInterimStatusViewModel
    {
        public UpdateInterimStatusViewModel()
        {
        }

        public UpdateInterimStatusViewModel(InterimStatus interimStatus, NotificationType notificationType)
        {
            NotificationId = interimStatus.NotificationId;
            NotificationStatus = interimStatus.NotificationStatus;
            IsInterim = interimStatus.IsInterim;
            NotificationType = notificationType;
        }

        public Guid NotificationId { get; set; }

        [Required]
        public bool? IsInterim { get; set; }

        public NotificationStatus NotificationStatus { get; set; }

        public NotificationType NotificationType { get; set; }

        public bool HasAcceptableStatus
        {
            get
            {
                return NotificationStatus == NotificationStatus.InAssessment ||
                       NotificationStatus == NotificationStatus.ReadyToTransmit ||
                       NotificationStatus == NotificationStatus.Transmitted ||
                       NotificationStatus == NotificationStatus.DecisionRequiredBy;
            }
        }    
    }
}