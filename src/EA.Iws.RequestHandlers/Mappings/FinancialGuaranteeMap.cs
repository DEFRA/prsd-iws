namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.FinancialGuarantee;
    using Core.Notification;
    using Domain.FinancialGuarantee;
    using Prsd.Core.Mapper;

    public class FinancialGuaranteeMap : IMapWithParameter<FinancialGuarantee, UKCompetentAuthority, FinancialGuaranteeData>
    {
        private readonly Domain.IWorkingDayCalculator workingDayCalculator;

        public FinancialGuaranteeMap(Domain.IWorkingDayCalculator workingDayCalculator)
        {
            this.workingDayCalculator = workingDayCalculator;
        }

        public FinancialGuaranteeData Map(FinancialGuarantee source, UKCompetentAuthority parameter)
        {
            if (source == null)
            {
                return new FinancialGuaranteeData
                {
                    IsEmpty = true
                };
            }

            return new FinancialGuaranteeData
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
                IsEmpty = false
            };
        }
    }
}
