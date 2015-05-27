namespace EA.Iws.Requests.Producers
{
    using System;

    public class UpdateProducerForNotification : AddProducerToNotification
    {
        public Guid ProducerId { get; set; }
    }
}