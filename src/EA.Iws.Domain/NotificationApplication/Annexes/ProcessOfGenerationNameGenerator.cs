namespace EA.Iws.Domain.NotificationApplication.Annexes
{
    public class ProcessOfGenerationNameGenerator : IAnnexNameGenerator
    {
        public string GetValue(NotificationApplication notification)
        {
            return string.Format("Process-of-generation-{0}", notification.NotificationNumber);
        }
    }
}