namespace EA.Iws.Requests.ImportNotificationAssessment.FinancialGuarantee
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportFinancialGuaranteePermissions.CanEditImportFinancialGuarantee)]
    public class SetCompletedDate : IRequest<bool>
    {
        public Guid ImportNotificationId { get; private set; }

        public DateTime Date { get; private set; }

        public SetCompletedDate(Guid importNotificationId, DateTime date)
        {
            ImportNotificationId = importNotificationId;
            Date = date;
        }
    }
}
