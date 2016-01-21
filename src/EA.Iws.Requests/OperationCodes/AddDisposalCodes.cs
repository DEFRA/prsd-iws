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
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class AddDisposalCodes : IRequest<Guid>
    {
        public AddDisposalCodes(List<DisposalCode> disposalCodes, Guid notificationId)
        {
            DisposalCodes = disposalCodes;
            NotificationId = notificationId;
        }

        public List<DisposalCode> DisposalCodes { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}
