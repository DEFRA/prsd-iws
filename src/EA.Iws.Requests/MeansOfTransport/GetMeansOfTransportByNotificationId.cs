namespace EA.Iws.Requests.MeansOfTransport
{
    using System;
    using System.Collections.Generic;
    using Core.MeansOfTransport;
    using Prsd.Core.Mediator;

    public class GetMeansOfTransportByNotificationId : IRequest<IList<MeansOfTransport>>
    {
        public Guid Id { get; private set; }

        public GetMeansOfTransportByNotificationId(Guid id)
        {
            Id = id;
        }
    }
}
