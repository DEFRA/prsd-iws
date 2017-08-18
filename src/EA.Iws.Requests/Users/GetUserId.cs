namespace EA.Iws.Requests.Users
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanReadUserData)]
    public class GetUserId : IRequest<Guid>
    {
        public GetUserId(string emailAddress)
        {
            EmailAddress = emailAddress;
        }

        public string EmailAddress { get; private set; }
    }
}