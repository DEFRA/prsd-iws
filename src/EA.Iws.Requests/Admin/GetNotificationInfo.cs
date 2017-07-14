namespace EA.Iws.Requests.Admin
{
    using Core.Admin;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanReadImportExportNotificationData)]
    public class GetNotificationInfo : IRequest<NotificationInfo>
    {
        public string Number { get; private set; }

        public GetNotificationInfo(string number)
        {
            Number = number;
        }
    }
}
