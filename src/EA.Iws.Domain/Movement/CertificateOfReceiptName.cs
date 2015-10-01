namespace EA.Iws.Domain.Movement
{
    using System.Threading.Tasks;
    using NotificationApplication;

    public class CertificateOfReceiptName
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly string nameFormat = "{0}-shipment-{1}-receipt";

        public CertificateOfReceiptName(INotificationApplicationRepository notificationApplicationRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<string> GetValue(Movement movement)
        {
            var notification = await notificationApplicationRepository.GetById(movement.NotificationId);
            var notificationNumber = notification.NotificationNumber.Replace(" ", string.Empty);
            return string.Format(nameFormat, notificationNumber, movement.Number);
        }
    }
}