namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Core.NotificationAssessment;

    public class MarkAsInterimViewModel
    {
        public MarkAsInterimViewModel()
        {
        }

        public MarkAsInterimViewModel(InterimStatus interimStatus)
        {
            NotificationId = interimStatus.NotificationId;
            NotificationStatus = interimStatus.NotificationStatus;
            IsInterim = interimStatus.IsInterim;
        }

        public Guid NotificationId { get; set; }

        [Required]
        public bool? IsInterim { get; set; }

        public NotificationStatus NotificationStatus { get; set; }
    }
}