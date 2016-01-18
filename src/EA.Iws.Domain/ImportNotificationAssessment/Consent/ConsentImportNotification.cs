namespace EA.Iws.Domain.ImportNotificationAssessment.Consent
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using ImportNotification;
    using Prsd.Core.Domain;

    [AutoRegister]
    public class ConsentImportNotification
    {
        private readonly IImportConsentRepository consentRepository;
        private readonly IImportNotificationAssessmentRepository assessmentRepository;
        private readonly IUserContext userContext;

        public ConsentImportNotification(IImportConsentRepository consentRepository,
            IImportNotificationAssessmentRepository assessmentRepository,
            IUserContext userContext)
        {
            this.consentRepository = consentRepository;
            this.assessmentRepository = assessmentRepository;
            this.userContext = userContext;
        }

        public async Task Consent(Guid notificationId,
            DateTimeOffsetRange dateRange,
            string conditions,
            DateTimeOffset consentedDate)
        {
            var assessment = await assessmentRepository.GetByNotification(notificationId);

            var consent = assessment.Consent(dateRange, conditions, userContext.UserId, consentedDate);

            consentRepository.Add(consent);
        }
    }
}
