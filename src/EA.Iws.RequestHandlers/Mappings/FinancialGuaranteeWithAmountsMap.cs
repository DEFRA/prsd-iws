namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.FinancialGuarantee;
    using Core.Notification;
    using Domain.FinancialGuarantee;
    using Prsd.Core.Mapper;

    public class FinancialGuaranteeWithAmountsMap : IMapWithParameter<FinancialGuarantee, UKCompetentAuthority, FinancialGuaranteeDataWithAmounts>
    {
        private readonly Domain.IWorkingDayCalculator workingDayCalculator;

        public FinancialGuaranteeWithAmountsMap(Domain.IWorkingDayCalculator workingDayCalculator)
        {
            this.workingDayCalculator = workingDayCalculator;
        }

        public FinancialGuaranteeDataWithAmounts Map(FinancialGuarantee source, UKCompetentAuthority parameter)
        {
            if (source == null)
            {
                return new FinancialGuaranteeDataWithAmounts
                {
                    IsEmpty = true
                };
            }

            return new FinancialGuaranteeDataWithAmounts
            {
                FinancialGuaranteeId = source.Id,
                Status = source.Status,
                CompletedDate = source.CompletedDate,
                DecisionRequiredDate = source.GetDecisionRequiredDate(workingDayCalculator, parameter),
                ReceivedDate = source.ReceivedDate,
                DecisionDate = source.DecisionDate,
                RefusalReason = source.RefusalReason,
                ActiveLoadsPermitted = source.ActiveLoadsPermitted,
                Decision = source.Decision,
                ReferenceNumber = source.ReferenceNumber,
                IsBlanketBond = source.IsBlanketBond.GetValueOrDefault(),
                CoverAmount = source.CoverAmount,
                CalculationContinued = source.CalculationContinued,
                IsEmpty = false
            };
        }
    }
}
