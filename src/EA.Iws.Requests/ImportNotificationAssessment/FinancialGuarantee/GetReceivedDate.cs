namespace EA.Iws.Requests.ImportNotificationAssessment.FinancialGuarantee
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportFinancialGuaranteePermissions.CanEditImportFinancialGuarantee)]
    public class GetReceivedDate : IRequest<DateTime?>
    {
        public Guid ImportNotificationId { get; private set; }

        public GetReceivedDate(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }
    }
}
