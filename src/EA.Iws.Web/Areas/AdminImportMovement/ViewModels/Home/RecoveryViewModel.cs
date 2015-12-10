namespace EA.Iws.Web.Areas.AdminImportMovement.ViewModels.Home
{
    using System;
    using Core.Shared;
    using Web.ViewModels.Shared;

    public class RecoveryViewModel : IComplete
    {
        public OptionalDateInputViewModel RecoveryDate { get; set; }

        public NotificationType NotificationType { get; set; }

        public RecoveryViewModel()
        {
            RecoveryDate = new OptionalDateInputViewModel(true);
        }

        public RecoveryViewModel(DateTimeOffset? recoveryDate, NotificationType notificationType)
        {
            NotificationType = notificationType;
            if (recoveryDate.HasValue)
            {
                RecoveryDate = new OptionalDateInputViewModel(recoveryDate.Value.DateTime, true);
            }
            else
            {
                RecoveryDate = new OptionalDateInputViewModel(true);
            }
        }

        public bool IsComplete()
        {
            return RecoveryDate.IsCompleted;
        }
    }
}