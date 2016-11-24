namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using Core.FinancialGuarantee;
    using Core.NotificationAssessment;
    using FinancialGuarantee;

    [AutoRegister]
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
            var financialGuaranteeCollection = await financialGuaranteeRepository.GetByNotificationId(notificationId);

            if (assessment.Status != NotificationStatus.Consented)
            {
                return false;
            }

            return
                financialGuaranteeCollection.FinancialGuarantees.Any(
                    fg => fg.Status == FinancialGuaranteeStatus.ApplicationComplete
                        || fg.Status == FinancialGuaranteeStatus.ApplicationReceived
                        || fg.Status == FinancialGuaranteeStatus.AwaitingApplication);
        }
    }
}