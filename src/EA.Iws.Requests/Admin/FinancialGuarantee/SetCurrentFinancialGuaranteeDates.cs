namespace EA.Iws.Requests.Admin.FinancialGuarantee
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(UserAdministrationPermissions.CanOverrideFinancialGuaranteeDates)]
    public class SetCurrentFinancialGuaranteeDates : IRequest<Unit>
    {
        public SetCurrentFinancialGuaranteeDates(Guid notificationId, DateTime? receivedDate, DateTime? completedDate,
            DateTime? decisionDate)
        {
            CompletedDate = completedDate;
            DecisionDate = decisionDate;
            NotificationId = notificationId;
            ReceivedDate = receivedDate;
        }

        public Guid NotificationId { get; private set; }

        public DateTime? ReceivedDate { get; private set; }

        public DateTime? CompletedDate { get; private set; }

        public DateTime? DecisionDate { get; private set; }
    }
}