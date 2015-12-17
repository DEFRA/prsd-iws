namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using Core.FinancialGuarantee;
    using Core.NotificationAssessment;
    using FinancialGuarantee;

    public class FinancialGuaranteeDecisionRequired
    {
        private readonly IFinancialGuaranteeRepository financialGuaranteeRepository;
        private readonly INotificationAssessmentRepository assessmentRepository;

        public FinancialGuaranteeDecisionRequired(INotificationAssessmentRepository assessmentRepository,
            IFinancialGuaranteeRepository financialGuaranteeRepository)
        {
            this.assessmentRepository = assessmentRepository;
            this.financialGuaranteeRepository = financialGuaranteeRepository;
        }

        public async Task<bool> Calculate(Guid notificationId)
        {
            var assessment = await assessmentRepository.GetByNotificationId(notificationId);
            var financialGuarantee = await financialGuaranteeRepository.GetByNotificationId(notificationId);

            if (assessment.Status != NotificationStatus.Consented)
            {
                return false;
            }

            if (financialGuarantee.Status != FinancialGuaranteeStatus.ApplicationComplete
                && financialGuarantee.Status != FinancialGuaranteeStatus.ApplicationReceived
                && financialGuarantee.Status != FinancialGuaranteeStatus.AwaitingApplication)
            {
                return false;
            }

            return true;
        }
    }
}