namespace EA.Iws.Domain
{
    public interface INotificationDocumentGenerator
    {
        byte[] GenerateNotificationDocument(NotificationApplication.NotificationApplication notification, NotificationApplication.ShipmentInfo shipmentInfo, TransportRoute.TransportRoute transportRoute);
    }
}