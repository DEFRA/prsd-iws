namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetTransactionById : IRequest<TransactionRecordData>
    {
        public Guid TransactionId { get; private set; }

        public GetTransactionById(Guid transactionId)
        {
            TransactionId = transactionId;
        }
    }
}
