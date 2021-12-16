namespace EA.Iws.Requests.Movement.PartialReject
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanEditExportMovementsInternal)]
    public class RecordParialOperationCompleteInternal : IRequest<bool>
    {
        public Guid Id { get; private set; }

        public DateTime? CompleteDate { get; private set; }

        public RecordParialOperationCompleteInternal(Guid id, DateTime? completeDate)
        {
            Id = id;
            CompleteDate = completeDate;
        }
    }
}
