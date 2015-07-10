namespace EA.Iws.Domain
{
    using Notification;

    public interface IDocumentGenerator
    {
        byte[] GenerateNotificationDocument(NotificationApplication notification);

        byte[] GenerateFinancialGuaranteeDocument();
    }
}