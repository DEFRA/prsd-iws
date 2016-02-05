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
        public AddDisposalCodes(Guid notificationId, IList<OperationCode> disposalCodes)
        {
            DisposalCodes = disposalCodes;
            NotificationId = notificationId;
        }

        public IList<OperationCode> DisposalCodes { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}
