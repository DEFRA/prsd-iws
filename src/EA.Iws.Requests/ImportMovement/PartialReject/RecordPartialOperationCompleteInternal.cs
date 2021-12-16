namespace EA.Iws.Requests.ImportMovement.PartialReject
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportMovementPermissions.CanEditImportMovements)]
    public class RecordPartialOperationCompleteInternal : IRequest<bool>
    {
        public Guid Id { get; private set; }

        public DateTime? CompleteDate { get; private set; }

        public RecordPartialOperationCompleteInternal(Guid id, DateTime? completeDate)
        {
            Id = id;
            CompleteDate = completeDate;
        }
    }
}
