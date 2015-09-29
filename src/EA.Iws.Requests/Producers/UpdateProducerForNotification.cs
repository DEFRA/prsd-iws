namespace EA.Iws.Requests.Producers
{
    using System;

    [NotificationReadOnlyAuthorize]
    public class UpdateProducerForNotification : AddProducerToNotification
    {
        public Guid ProducerId { get; set; }
    }
}