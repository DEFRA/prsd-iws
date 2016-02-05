namespace EA.Iws.Requests.OperationCodes
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.OperationCodes;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetOperationCodesByNotificationId : IRequest<IList<OperationCode>>
    {
        public Guid NotificationId { get; set; }

        public GetOperationCodesByNotificationId(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
