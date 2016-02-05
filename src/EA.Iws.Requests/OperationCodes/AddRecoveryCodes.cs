namespace EA.Iws.Requests.OperationCodes
{
    using System;
    using System.Collections.Generic;
    using Authorization;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.OperationCodes;
    using Prsd.Core.Mediator;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class AddRecoveryCodes : IRequest<Guid>
    {
        public AddRecoveryCodes(Guid notificationId, IList<OperationCode> recoveryCodes)
        {
            RecoveryCodes = recoveryCodes;
            NotificationId = notificationId;
        }

        public IList<OperationCode> RecoveryCodes { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}