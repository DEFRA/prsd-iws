namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetExportNotificationTransactions : IRequest<IList<TransactionRecordData>>
    {
        public Guid NotificationId { get; private set; }

        public GetExportNotificationTransactions(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
