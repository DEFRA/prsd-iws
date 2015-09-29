namespace EA.Iws.Requests.Carriers
{
    using System;
    using Security;

    [NotificationReadOnlyAuthorize]
    public class UpdateCarrierForNotification : AddCarrierToNotification
    {
        public Guid CarrierId { get; set; }
    }
}