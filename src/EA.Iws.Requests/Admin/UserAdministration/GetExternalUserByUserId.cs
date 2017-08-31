namespace EA.Iws.Requests.Admin.UserAdministration
{
    using System;
    using Core.Admin;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanReadUserData)]
    public class GetExternalUserByUserId : IRequest<ExternalUserData>
    {
        public GetExternalUserByUserId(Guid userId)
        {
            UserId = userId;
        }

        public Guid UserId { get; private set; }
    }
}