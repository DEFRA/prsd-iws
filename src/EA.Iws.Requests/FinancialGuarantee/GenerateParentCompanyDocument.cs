namespace EA.Iws.Requests.FinancialGuarantee
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanReadExportNotification)]
    public class GenerateParentCompanyDocument : IRequest<byte[]>
    {
    }
}
