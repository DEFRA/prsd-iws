namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Facility
{
    using EA.Iws.Core.NotificationAssessment;
    using EA.Iws.Core.Shared;
    using EA.Iws.Requests.Facilities;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class MarkAsInterimViewModel
    {
        public MarkAsInterimViewModel()
        {
        }

        public MarkAsInterimViewModel(InterimStatus interimStatus)
        {
            NotificationId = interimStatus.NotificationId;
            IsInterim = interimStatus.IsInterim;
        }

        public Guid NotificationId { get; set; }

        [Required]
        public bool? IsInterim { get; set; }

        public NotificationType NotificationType { get; set; }

        public MarkAsInterimToNotification ToRequest()
        {
            return new MarkAsInterimToNotification(NotificationId, IsInterim.Value);
        }
    }
}