namespace EA.Iws.Requests.Carriers
{
    using System;

    public class UpdateCarrierForNotification : AddCarrierToNotification
    {
        public Guid CarrierId { get; set; }
    }
}