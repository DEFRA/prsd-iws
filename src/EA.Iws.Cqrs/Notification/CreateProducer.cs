namespace EA.Iws.Cqrs.Notification
{
    using System;
    using Prsd.Core.Mediator;

    public class CreateProducer : IRequest<Guid>
    {
        public CreateProducer(ProducerData producer)
        {
            Producer = producer;
        }

        public ProducerData Producer { get; private set; }
    }
}