namespace EA.Iws.Web.Areas.AdminMovement.ViewModels.InternalCapture
{
    using Core.Shared;
    using Web.ViewModels.Shared;

    public class RecoveryViewModel : IComplete
    {
        public OptionalDateInputViewModel RecoveryDate { get; set; }

        public NotificationType NotificationType { get; set; }

        public RecoveryViewModel()
        {
            RecoveryDate = new OptionalDateInputViewModel();
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