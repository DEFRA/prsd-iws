namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.Admin;
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
                    Status = FinancialGuaranteeStatus.AwaitingApplication
                };
            }

            return new FinancialGuaranteeData
            {
                Status = source.Status,
                CompletedDate = source.CompletedDate,
                DecisionRequiredDate = source.GetDecisionRequiredDate(workingDayCalculator, parameter),
                ReceivedDate = source.ReceivedDate,
                DecisionDate = source.DecisionDate,
                RefusalReason = source.RefusalReason,
                ActiveLoadsPermitted = source.ActiveLoadsPermitted,
                Decision = GetDecision(source),
                ReferenceNumber = source.ReferenceNumber,
                IsBlanketBond = source.IsBlanketBond.GetValueOrDefault()
            };
        }

        private FinancialGuaranteeDecision? GetDecision(FinancialGuarantee guarantee)
        {
            if (guarantee.Status == FinancialGuaranteeStatus.Approved)
            {
                return FinancialGuaranteeDecision.Approved;
            }
            else if (guarantee.Status == FinancialGuaranteeStatus.Refused)
            {
                return FinancialGuaranteeDecision.Refused;
            }
            else if (guarantee.Status == FinancialGuaranteeStatus.Released)
            {
                return FinancialGuaranteeDecision.Released;
            }

            return null;
        }
    }
}
