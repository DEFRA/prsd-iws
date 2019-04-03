namespace EA.Iws.Requests.Users
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GetUserIsInternal : IRequest<bool>
    {
        public GetUserIsInternal()
        {
        }
    }
}
