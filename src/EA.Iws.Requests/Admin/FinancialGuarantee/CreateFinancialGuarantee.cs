namespace EA.Iws.Requests.Admin.FinancialGuarantee
{
    using System;
    using Prsd.Core.Mediator;

    public class CreateFinancialGuarantee : IRequest<Guid>
    {
        public CreateFinancialGuarantee(Guid notificationId, DateTime receivedDate)
        {
            NotificationId = notificationId;
            ReceivedDate = receivedDate;
        }

        public Guid NotificationId { get; private set; }

        public DateTime ReceivedDate { get; private set; }
    }
}