namespace EA.Iws.Domain.NotificationApplication.Annexes
{
    public interface IAnnexNameGenerator
    {
        string GetValue(NotificationApplication notification);
    }
}