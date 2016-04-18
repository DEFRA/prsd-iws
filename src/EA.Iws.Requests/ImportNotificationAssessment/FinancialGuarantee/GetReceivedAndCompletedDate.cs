namespace EA.Iws.Requests.ImportNotificationAssessment.FinancialGuarantee
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportNotificationAssessment.FinancialGuarantee;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportFinancialGuaranteePermissions.CanEditImportFinancialGuarantee)]
    public class GetReceivedAndCompletedDate : IRequest<ReceivedAndCompletedDateData>
    {
        public Guid ImportNotificationId { get; private set; }

        public GetReceivedAndCompletedDate(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }
    }
}
