namespace EA.Iws.Domain.Movement
{
    using System.Threading.Tasks;
    using NotificationApplication;

    public class CertificateOfRecoveryNameGenerator : ICertificateNameGenerator
    {
        private readonly INotificationApplicationRepository notificationRepository;
        private readonly string nameFormat = "{0}-shipment-{1}-{2}-receipt";

        public CertificateOfRecoveryNameGenerator(INotificationApplicationRepository notificationRepository)
        {
            this.notificationRepository = notificationRepository;
        }

        public async Task<string> GetValue(Movement movement)
        {
            var notification = await notificationRepository.GetById(movement.NotificationId);
            var notificationNumber = notification.NotificationNumber.Replace(" ", string.Empty);
            var notificationType = notification.NotificationType.ToString().ToLowerInvariant();

            return string.Format(nameFormat, notificationNumber, movement.Number, notificationType);
        }
    }
}
