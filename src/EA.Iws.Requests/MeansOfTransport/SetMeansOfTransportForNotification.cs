namespace EA.Iws.Requests.MeansOfTransport
{
    using System;
    using System.Collections.Generic;
    using Core.MeansOfTransport;
    using Prsd.Core.Mediator;

    public class SetMeansOfTransportForNotification : IRequest<Guid>
    {
        public Guid Id { get; private set; }

        public IList<MeansOfTransport> MeansOfTransport { get; private set; }

        public SetMeansOfTransportForNotification(Guid id, IList<MeansOfTransport> meansOfTransport)
        {
            Id = id;
            MeansOfTransport = meansOfTransport;
        }
    }
}
