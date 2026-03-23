namespace EA.Iws.Requests.OperationCodes
{
    using EA.Iws.Core.Authorization;
    using EA.Iws.Core.Authorization.Permissions;
    using EA.Iws.Core.OperationCodes;
    using EA.Iws.Core.Shared;
    using EA.Prsd.Core.Mediator;
    using System.Collections.Generic;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetOperationCodesByNotificationType : IRequest<IList<OperationCode>>
    {
        public NotificationType NotificationType { get; set; }

        public bool IsInterim { get; set; }

        public GetOperationCodesByNotificationType(NotificationType notificationType, bool isInterim)
        {
            NotificationType = notificationType;
            IsInterim = isInterim;
        }
    }
}
