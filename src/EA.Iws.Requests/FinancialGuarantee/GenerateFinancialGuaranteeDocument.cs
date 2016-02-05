namespace EA.Iws.Requests.FinancialGuarantee
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Notification;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GenerateFinancialGuaranteeDocument : IRequest<byte[]>
    {
        public UKCompetentAuthority CompetentAuthority { get; private set; }

        public GenerateFinancialGuaranteeDocument(UKCompetentAuthority competentAuthority)
        {
            CompetentAuthority = competentAuthority;
        }
    }
}
