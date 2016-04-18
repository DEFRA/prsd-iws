namespace EA.Iws.Requests.Admin
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(SystemConfigurationPermissions.CanSendTestEmail)]
    public class SendTestEmail : IRequest<bool>
    {
        public SendTestEmail(string emailTo)
        {
            EmailTo = emailTo;
        }

        public string EmailTo { get; private set; }
    }
}