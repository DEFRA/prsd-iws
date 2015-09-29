namespace EA.Iws.Requests.Carriers
{
    using System;

    [NotificationReadOnlyAuthorize]
    public class UpdateCarrierForNotification : AddCarrierToNotification
    {
        public Guid CarrierId { get; set; }
    }
}