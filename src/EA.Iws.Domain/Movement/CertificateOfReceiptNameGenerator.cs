namespace EA.Iws.Domain.Movement
{
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using NotificationApplication;

    [AutoRegister]
    public class CertificateOfReceiptNameGenerator : ICertificateNameGenerator
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly string nameFormat = "{0}-shipment-{1}-receipt";

        public CertificateOfReceiptNameGenerator(INotificationApplicationRepository notificationApplicationRepository)
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