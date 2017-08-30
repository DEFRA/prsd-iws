namespace EA.Iws.Requests.NotificationMovements
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovements)]
    public class GetSummaryAndTable : IRequest<NotificationMovementsSummaryAndTable>
    {
        public Guid Id { get; private set; }

        public MovementStatus? Status { get; private set; }

        public int PageNumber { get; private set; }

        public GetSummaryAndTable(Guid id, MovementStatus? status, int pageNumber)
        {
            Id = id;
            Status = status;
            PageNumber = pageNumber;
        }
    }
}
