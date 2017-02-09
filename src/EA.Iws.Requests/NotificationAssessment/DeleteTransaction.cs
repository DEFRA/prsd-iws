namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(UserAdministrationPermissions.CanDeleteTransaction)]
    public class DeleteTransaction : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public Guid TransactionId { get; private set; }

        public DeleteTransaction(Guid notificationId, Guid transactionId)
        {
            NotificationId = notificationId;
            TransactionId = transactionId;
        }
    }
}
