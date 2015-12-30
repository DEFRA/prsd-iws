namespace EA.Iws.Domain.NotificationApplication.Annexes
{
    public class TechnologyEmployedNameGenerator : IAnnexNameGenerator
    {
        public string GetValue(NotificationApplication notification)
        {
            return string.Format("Technology-employed-{0}", notification.NotificationNumber);
        }
    }
}