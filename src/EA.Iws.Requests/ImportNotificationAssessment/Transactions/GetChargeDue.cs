namespace EA.Iws.Requests.ImportNotificationAssessment.Transactions
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanReadImportNotificationAssessment)]
    public class GetChargeDue : IRequest<decimal>
    {
        public GetChargeDue(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }

        public Guid ImportNotificationId { get; private set; }
    }
}