namespace EA.Iws.Requests.Notification
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Notification;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GeneratePostageLabel : IRequest<byte[]>
    {
        public UKCompetentAuthority CompetentAuthority { get; private set; }

        public GeneratePostageLabel(UKCompetentAuthority competentAuthority)
        {
            CompetentAuthority = competentAuthority;
        }
    }
}
