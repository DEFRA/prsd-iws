namespace EA.Iws.Web.Areas.AdminImportNotificationMovements.ViewModels.Capture
{
    using System;
    using Core.Shared;
    using Web.ViewModels.Shared;
    public class RecoveryViewModel
    {
        public MaskedDateInputViewModel RecoveryDate { get; set; }

        public NotificationType NotificationType { get; set; }

        public RecoveryViewModel()
        {
            RecoveryDate = new MaskedDateInputViewModel();
        }

        public RecoveryViewModel(DateTimeOffset? recoveryDate, NotificationType notificationType)
        {
            NotificationType = notificationType;
            if (recoveryDate.HasValue)
            {
                RecoveryDate = new MaskedDateInputViewModel(recoveryDate.Value.DateTime);
            }
            else
            {
                RecoveryDate = new MaskedDateInputViewModel();
            }
        }

        public bool IsComplete()
        {
            return RecoveryDate.IsCompleted;
        }
    }
}