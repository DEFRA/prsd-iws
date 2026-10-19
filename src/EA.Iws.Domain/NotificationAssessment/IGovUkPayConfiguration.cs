namespace EA.Iws.Domain.NotificationAssessment
{
    public interface IGovUkPayConfiguration
    {
        string Description { get; }

        string ReturnUrlFormat { get; }
    }
}