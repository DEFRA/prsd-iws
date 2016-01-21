namespace EA.Iws.Requests.OperationCodes
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.OperationCodes;
    using Prsd.Core.Mediator;
    using Security;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class AddRecoveryCodes : IRequest<Guid>
    {
        public AddRecoveryCodes(List<RecoveryCode> recoveryCodes, Guid notificationId)
        {
            RecoveryCodes = recoveryCodes;
            NotificationId = notificationId;
        }

        public List<RecoveryCode> RecoveryCodes { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}
