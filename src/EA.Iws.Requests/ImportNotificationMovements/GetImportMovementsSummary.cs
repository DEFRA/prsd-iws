namespace EA.Iws.Requests.ImportNotificationMovements
{
    using System;
    using Core.ImportNotificationMovements;
    using Prsd.Core.Mediator;

    public class GetImportMovementsSummary : IRequest<Summary>
    {
        public Guid ImportNotificationId { get; private set; }

        public GetImportMovementsSummary(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }
    }
}
