namespace EA.Iws.Requests.NotificationAssessment.Payment
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class ProcessGovUkPaymentReturn : IRequest<ProcessGovUkPaymentReturnResult>
    {
        public string SecureToken { get; private set; }

        public ProcessGovUkPaymentReturn(string secureToken)
        {
            SecureToken = secureToken;
        }
    }
}