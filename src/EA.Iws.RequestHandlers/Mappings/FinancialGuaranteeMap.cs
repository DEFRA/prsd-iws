namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Admin;
    using Domain;
    using Domain.FinancialGuarantee;
    using Prsd.Core.Mapper;

    public class FinancialGuaranteeMap : IMapWithParameter<FinancialGuarantee, UKCompetentAuthority, FinancialGuaranteeData>
    {
        private readonly IWorkingDayCalculator workingDayCalculator;

        public FinancialGuaranteeMap(IWorkingDayCalculator workingDayCalculator)
        {
            this.workingDayCalculator = workingDayCalculator;
        }

        public FinancialGuaranteeData Map(FinancialGuarantee source, UKCompetentAuthority parameter)
        {
            return new FinancialGuaranteeData
            {
                Status = source.Status,
                CompletedDate = source.CompletedDate,
                DecisionRequiredDate = source.GetDecisionRequiredDate(workingDayCalculator, parameter),
                ReceivedDate = source.ReceivedDate
            };
        }
    }
}
