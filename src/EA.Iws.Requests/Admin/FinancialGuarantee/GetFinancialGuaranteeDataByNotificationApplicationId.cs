namespace EA.Iws.Requests.Admin.FinancialGuarantee
{
    using System;
    using Core.Admin;
    using Prsd.Core.Mediator;

    public class GetFinancialGuaranteeDataByNotificationApplicationId : IRequest<FinancialGuaranteeData>
    {
        public Guid Id { get; private set; }

        public GetFinancialGuaranteeDataByNotificationApplicationId(Guid id)
        {
            Id = id;
        }
    }
}
