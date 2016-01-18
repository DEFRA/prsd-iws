namespace EA.Iws.Domain.NotificationApplication.Annexes
{
    using Core.ComponentRegistration;

    [AutoRegister]
    public class ProcessOfGenerationNameGenerator : IAnnexNameGenerator
    {
        public string GetValue(NotificationApplication notification)
        {
            return string.Format("Process-of-generation-{0}", notification.NotificationNumber);
        }
    }
}