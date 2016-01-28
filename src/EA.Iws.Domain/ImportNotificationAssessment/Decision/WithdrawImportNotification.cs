namespace EA.Iws.Domain.ImportNotificationAssessment.Decision
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using ImportNotification;

    [AutoRegister]
    public class WithdrawImportNotification
    {
        private readonly IImportNotificationAssessmentRepository assessmentRepository;
        private readonly IImportWithdrawnRepository withdrawnRepository;

        public WithdrawImportNotification(IImportNotificationAssessmentRepository assessmentRepository,
            IImportWithdrawnRepository withdrawnRepository)
        {
            this.assessmentRepository = assessmentRepository;
            this.withdrawnRepository = withdrawnRepository;
        }

        public async Task<ImportWithdrawn> Withdraw(Guid importNotificationId, DateTime date, string reasons)
        {
            var assessment = await assessmentRepository.GetByNotification(importNotificationId);

            var withdrawal = assessment.Withdraw(date, reasons);

            withdrawnRepository.Add(withdrawal);

            return withdrawal;
        } 
    }
}
