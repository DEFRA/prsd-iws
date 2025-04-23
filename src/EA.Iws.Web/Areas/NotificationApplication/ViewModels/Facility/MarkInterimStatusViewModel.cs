namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Facility
{
    using EA.Iws.Core.NotificationAssessment;
    using EA.Iws.Core.Shared;
    using EA.Iws.Requests.Facilities;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class MarkInterimStatusViewModel
    {
        public MarkInterimStatusViewModel()
        {
        }

        public MarkInterimStatusViewModel(InterimStatus interimStatus, NotificationType notificationType)
        {
            NotificationId = interimStatus.NotificationId;
            IsInterim = interimStatus.IsInterim;
            NotificationType = notificationType;
        }

        public Guid NotificationId { get; set; }

        [Required]
        public bool? IsInterim { get; set; }

        public NotificationType NotificationType { get; set; }

        public MarkInterimStatusToNotification ToRequest()
        {
            return new MarkInterimStatusToNotification(NotificationId, IsInterim.Value);
        }
    }
}