namespace EA.Iws.Domain.NotificationApplication.Annexes
{
    using Core.ComponentRegistration;
    using FileStore;

    [AutoRegister]
    public class AnnexFactory
    {
        public File CreateForNotification(IAnnexNameGenerator annexNameGenerator, NotificationApplication notification,
            byte[] annexBytes, string fileType)
        {
            var name = annexNameGenerator.GetValue(notification);

            return new File(name, fileType, annexBytes);
        }
    }
}
