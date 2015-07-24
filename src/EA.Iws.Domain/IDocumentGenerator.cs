namespace EA.Iws.Domain
{
    public interface IDocumentGenerator
    {
        byte[] GenerateNotificationDocument(NotificationApplication.NotificationApplication notification);

        byte[] GenerateFinancialGuaranteeDocument();
    }
}