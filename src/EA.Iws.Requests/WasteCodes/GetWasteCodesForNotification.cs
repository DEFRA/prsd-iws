namespace EA.Iws.Requests.WasteCodes
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.WasteCodes;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetWasteCodesForNotification : IRequest<WasteCodeData[]>
    {
        public GetWasteCodesForNotification(Guid notificationId, CodeType codeType)
        {
            NotificationId = notificationId;
            CodeType = codeType;
        }

        public Guid NotificationId { get; private set; }

        public CodeType CodeType { get; private set; }
    }
}