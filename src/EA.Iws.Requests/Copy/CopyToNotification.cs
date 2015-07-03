namespace EA.Iws.Requests.Copy
{
    using System;
    using Prsd.Core.Mediator;

    public class CopyToNotification : IRequest<bool>
    {
        public Guid SourceId { get; private set; }

        public Guid DestinationId { get; private set; }

        public CopyToNotification(Guid sourceId, Guid destinationId)
        {
            SourceId = sourceId;
            DestinationId = destinationId;
        }
    }
}
