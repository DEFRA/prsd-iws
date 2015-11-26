namespace EA.Iws.Domain.NotificationConsent
{
    using System;
    using System.Threading.Tasks;
    using NotificationAssessment;
    using Prsd.Core.Domain;

    public class ConsentNotification
    {
        private readonly INotificationConsentRepository consentRepository;
        private readonly INotificationAssessmentRepository assessmentRepository;
        private readonly IUserContext userContext;

        public ConsentNotification(INotificationConsentRepository consentRepository, 
            INotificationAssessmentRepository assessmentRepository,
            IUserContext userContext)
        {
            this.consentRepository = consentRepository;
            this.assessmentRepository = assessmentRepository;
            this.userContext = userContext;
        }

        public async Task Consent(Guid notificationId, 
            DateRange dateRange, 
            string conditions,
            DateTime consentedDate)
        {
            var assessment = await assessmentRepository.GetByNotificationId(notificationId);

            var consent = assessment.Consent(dateRange, conditions, userContext.UserId, consentedDate);

            consentRepository.Add(consent);
        }
    }
}
