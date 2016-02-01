namespace EA.Iws.Requests.ImportNotificationAssessment.FinancialGuarantee
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportFinancialGuaranteePermissions.CanEditImportFinancialGuarantee)]
    public class SetReceivedAndCompletedDate : IRequest<bool>
    {
        public Guid ImportNotificationId { get; private set; }

        public DateTime CompletedDate { get; private set; }

        public DateTime ReceivedDate { get; private set; }

        public SetReceivedAndCompletedDate(Guid importNotificationId, DateTime completedDate, DateTime receivedDate)
        {
            ImportNotificationId = importNotificationId;
            CompletedDate = completedDate;
            ReceivedDate = receivedDate;
        }
    }
}
