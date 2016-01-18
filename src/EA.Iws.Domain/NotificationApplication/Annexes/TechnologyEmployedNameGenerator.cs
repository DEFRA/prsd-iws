namespace EA.Iws.Domain.NotificationApplication.Annexes
{
    using Core.ComponentRegistration;

    [AutoRegister]
    public class TechnologyEmployedNameGenerator : IAnnexNameGenerator
    {
        public string GetValue(NotificationApplication notification)
        {
            return string.Format("Technology-employed-{0}", notification.NotificationNumber);
        }
    }
}