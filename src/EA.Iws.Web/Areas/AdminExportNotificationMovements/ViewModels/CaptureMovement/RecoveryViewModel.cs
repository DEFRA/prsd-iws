namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.CaptureMovement
{
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

        public bool IsComplete()
        {
            if (RecoveryDate != null)
            {
                return RecoveryDate.IsCompleted;
            }

            return false;
        }
    }
}