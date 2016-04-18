namespace EA.Iws.Requests.Admin.UserAdministration
{
    using Core.Admin;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanReadUserData)]
    public class GetUserById : IRequest<ChangeUserData>
    {
        public string UserId { get; private set; }

        public GetUserById(string userId)
        {
            UserId = userId;
        }
    }
}
