namespace EA.Iws.Requests.TechnologyEmployed
{
    using System;
    using Core.TechnologyEmployed;
    using Prsd.Core.Mediator;

    public class GetTechnologyEmployed : IRequest<TechnologyEmployedData>
    {
        public GetTechnologyEmployed(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}