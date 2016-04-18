namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.NotificationAssessment;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetNotificationIdAndStatusByNumber : IRequest<Tuple<Guid?, NotificationStatus?>>
    {
        public string Number { get; private set; }

        public GetNotificationIdAndStatusByNumber(string number)
        {
            Number = number;
        }
    }
}
