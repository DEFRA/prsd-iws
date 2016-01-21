namespace EA.Iws.Requests.Movement.Complete
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanEditExportMovementsInternal)]
    public class RecordOperationCompleteInternal : IRequest<bool>
    {
        public Guid Id { get; private set; }

        public DateTime CompleteDate { get; private set; }

        public RecordOperationCompleteInternal(Guid id, DateTime completeDate)
        {
            Id = id;
            CompleteDate = completeDate;
        }
    }
}