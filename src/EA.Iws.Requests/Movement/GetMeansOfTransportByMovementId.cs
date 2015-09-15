namespace EA.Iws.Requests.Movement
{
    using System;
    using System.Collections.Generic;
    using Core.MeansOfTransport;
    using Prsd.Core.Mediator;

    public class GetMeansOfTransportByMovementId : IRequest<IList<MeansOfTransport>>
    {
        public Guid Id { get; private set; }

        public GetMeansOfTransportByMovementId(Guid id)
        {
            Id = id;
        }
    }
}
