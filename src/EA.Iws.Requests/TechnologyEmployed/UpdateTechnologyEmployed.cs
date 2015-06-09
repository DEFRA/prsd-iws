namespace EA.Iws.Requests.TechnologyEmployed
{
    using System;
    using Prsd.Core.Mediator;

    public class UpdateTechnologyEmployed : IRequest<Guid>
    {
        public Guid NotificationId { get; private set; }
        public bool AnnexProvided { get; private set; }
        public string Details { get; private set; }

        public UpdateTechnologyEmployed(Guid notificationId, bool annexProvided, string details)
        {
            NotificationId = notificationId;
            AnnexProvided = annexProvided;
            Details = details;
        }
    }
}
