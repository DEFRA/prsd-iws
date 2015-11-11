namespace EA.Iws.Requests.NotificationMovements.Create
{
    using System;
    using System.Collections.Generic;
    using Core.MeansOfTransport;
    using Prsd.Core.Mediator;

    public class GetMeansOfTransport : IRequest<IList<MeansOfTransport>>
    {
        public Guid NotificationId { get; private set; }

        public GetMeansOfTransport(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}