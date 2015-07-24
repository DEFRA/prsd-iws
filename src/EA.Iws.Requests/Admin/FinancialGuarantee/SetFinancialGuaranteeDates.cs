namespace EA.Iws.Requests.Admin.FinancialGuarantee
{
    using System;
    using Prsd.Core.Mediator;

    public class SetFinancialGuaranteeDates : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public DateTime? ReceivedDate { get; private set; }

        public DateTime? CompletedDate { get; private set; }

        public SetFinancialGuaranteeDates(Guid notificationId, DateTime? receivedDate, DateTime? completedDate)
        {
            NotificationId = notificationId;
            ReceivedDate = receivedDate;
            CompletedDate = completedDate;
        }
    }
}
