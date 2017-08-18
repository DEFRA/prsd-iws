namespace EA.Iws.Requests.Users
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanReadUserData)]
    public class ExternalUserExists : IRequest<bool>
    {
        public ExternalUserExists(string emailAddress)
        {
            EmailAddress = emailAddress;
        }

        public string EmailAddress { get; private set; }
    }
}