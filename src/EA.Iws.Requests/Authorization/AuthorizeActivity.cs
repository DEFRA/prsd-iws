namespace EA.Iws.Requests.Authorization
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanAuthorizeActivity)]
    public class AuthorizeActivity : IRequest<bool>
    {
        public string Activity { get; private set; }

        public AuthorizeActivity(string activity)
        {
            Activity = activity;
        }
    }
}