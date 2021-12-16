namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.CaptureMovement
{
    using Core.Shared;
    using Web.ViewModels.Shared;

    public class RecoveryViewModel
    {
        public MaskedDateInputViewModel RecoveryDate { get; set; }

        public NotificationType NotificationType { get; set; }

        public bool IsShipmentFullRejected { get; set; }

        public RecoveryViewModel()
        {
            RecoveryDate = new MaskedDateInputViewModel();
            IsShipmentFullRejected = false;
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