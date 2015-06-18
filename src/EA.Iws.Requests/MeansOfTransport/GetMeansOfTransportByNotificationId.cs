namespace EA.Iws.Requests.MeansOfTransport
{
    using System;
    using System.Collections.Generic;
    using Prsd.Core.Mediator;

    public class GetMeansOfTransportByNotificationId : IRequest<IList<Core.MeansOfTransport.MeansOfTransport>>
    {
        public Guid Id { get; private set; }

        public GetMeansOfTransportByNotificationId(Guid id)
        {
            Id = id;
        }
    }
}
