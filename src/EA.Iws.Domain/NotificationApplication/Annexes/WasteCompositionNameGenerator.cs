namespace EA.Iws.Domain.NotificationApplication.Annexes
{
    using Core.ComponentRegistration;

    [AutoRegister]
    public class WasteCompositionNameGenerator : IAnnexNameGenerator
    {
        public string GetValue(NotificationApplication notification)
        {
            return string.Format("Waste-composition-{0}", notification.NotificationNumber);
        }
    }
}