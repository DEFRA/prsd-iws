namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Admin;
    using Domain.FinancialGuarantee;
    using Prsd.Core.Mapper;

    public class FinancialGuaranteeMap : IMap<FinancialGuarantee, FinancialGuaranteeData>
    {
        public FinancialGuaranteeData Map(FinancialGuarantee source)
        {
            return new FinancialGuaranteeData
            {
                Status = source.Status,
                CompletedDate = source.CompletedDate,
                DecisionRequiredDate = source.DecisionRequiredDate,
                ReceivedDate = source.ReceivedDate
            };
        }
    }
}
