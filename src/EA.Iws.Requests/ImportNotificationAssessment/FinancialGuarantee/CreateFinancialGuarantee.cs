namespace EA.Iws.Requests.ImportNotificationAssessment.FinancialGuarantee
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportFinancialGuaranteePermissions.CanEditImportFinancialGuarantee)]
    public class CreateFinancialGuarantee : IRequest<bool>
    {
        public Guid ImportNotificationId { get; private set; }

        public DateTime ReceivedDate { get; private set; }

        public CreateFinancialGuarantee(Guid importNotificationId, DateTime receivedDate)
        {
            ImportNotificationId = importNotificationId;
            ReceivedDate = receivedDate;
        }
    }
}
