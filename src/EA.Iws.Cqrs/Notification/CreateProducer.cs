namespace EA.Iws.Cqrs.Notification
{
    using System;
    using Api.Client.Entities;
    using Core.Cqrs;

    public class CreateProducer : ICommand
    {
        public ProducerData Producer { get; set; }
        
        public CreateProducer(ProducerData producer)
        {
            Producer = producer;
        }
    }
}
