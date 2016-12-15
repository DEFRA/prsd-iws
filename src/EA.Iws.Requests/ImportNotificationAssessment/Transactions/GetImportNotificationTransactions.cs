namespace EA.Iws.Requests.ImportNotificationAssessment.Transactions
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(UserAdministrationPermissions.CanDeleteTransaction)]
    public class GetImportNotificationTransactions : IRequest<IList<TransactionRecordData>>
    {
        public Guid NotificationId { get; private set; }

        public GetImportNotificationTransactions(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
