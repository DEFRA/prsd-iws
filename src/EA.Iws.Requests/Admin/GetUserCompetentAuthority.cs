namespace EA.Iws.Requests.Admin
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Notification;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanReadUserData)]
    public class GetUserCompetentAuthority : IRequest<UKCompetentAuthority>
    {
    }
}