namespace EA.Iws.Requests.Admin.FinancialGuarantee
{
    using System;
    using Core.FinancialGuarantee;
    using Prsd.Core.Mediator;

    public class GetCurrentFinancialGuaranteeDetails : IRequest<CurrentFinancialGuaranteeDetails>
    {
        public GetCurrentFinancialGuaranteeDetails(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public Guid NotificationId { get; private set; }
    }
}