namespace EA.Iws.Requests.ImportNotificationAssessment.Transactions
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(UserAdministrationPermissions.CanDeleteTransaction)]
    public class DeleteTransaction : IRequest<bool>
    {
        public Guid TransactionId { get; private set; }

        public DeleteTransaction(Guid transactionId)
        {
            TransactionId = transactionId;
        }
    }
}
