namespace EA.Iws.Requests.NotificationMovements
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanEditExportMovements)]
    public class GetSummaryAndTable : IRequest<NotificationMovementsSummaryAndTable>
    {
        public Guid Id { get; private set; }

        public MovementStatus? Status { get; private set; }

        public GetSummaryAndTable(Guid id, MovementStatus? status)
        {
            Id = id;
            Status = status;
        }
    }
}
