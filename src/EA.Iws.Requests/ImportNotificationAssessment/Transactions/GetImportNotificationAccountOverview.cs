namespace EA.Iws.Requests.ImportNotificationAssessment.Transactions
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportNotificationAssessment.Transactions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanReadImportNotificationAssessment)]
    public class GetImportNotificationAccountOverview : IRequest<AccountOverviewData>
    {
        public Guid ImportNotificationId { get; private set; }

        public GetImportNotificationAccountOverview(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }
    }
}
