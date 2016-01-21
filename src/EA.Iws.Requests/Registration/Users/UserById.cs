namespace EA.Iws.Requests.Registration.Users
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Registration.Users;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanReadUserData)]
    public class UserById : IRequest<User>
    {
        public readonly string Id;

        public UserById(string id)
        {
            Id = id;
        }

        public UserById(Guid id)
        {
            Id = id.ToString();
        }
    }
}