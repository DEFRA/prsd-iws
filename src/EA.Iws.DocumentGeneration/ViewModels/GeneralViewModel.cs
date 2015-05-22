namespace EA.Iws.DocumentGeneration.ViewModels
{
    using Domain.Notification;

    internal class GeneralViewModel
    {
        public GeneralViewModel(NotificationApplication notification)
        {
            Number = notification.NotificationNumber;
            IsDisposal = notification.NotificationType.Equals(NotificationType.Disposal);
            IsRecovery = notification.NotificationType.Equals(NotificationType.Recovery);
        }

        public string Number { get; private set; }

        public bool IsDisposal { get; private set; }

        public bool IsRecovery { get; private set; }
    }
}