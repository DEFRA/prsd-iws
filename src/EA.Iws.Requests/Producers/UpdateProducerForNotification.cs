namespace EA.Iws.Requests.Producers
{
    using System;
    using Security;

    [NotificationReadOnlyAuthorize]
    public class UpdateProducerForNotification : AddProducerToNotification
    {
        public Guid ProducerId { get; set; }
    }
}