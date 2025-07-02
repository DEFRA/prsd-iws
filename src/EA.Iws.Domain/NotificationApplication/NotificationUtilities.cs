namespace EA.Iws.Domain.NotificationApplication
{
    using Core.ComponentRegistration;
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.NotificationAssessment;
    using EA.Iws.Domain.NotificationAssessment;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [AutoRegister]
    public class NotificationUtilities : INotificationUtilities
    {
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public NotificationUtilities(INotificationApplicationRepository notificationApplicationRepository,
                                     INotificationAssessmentRepository notificationAssessmentRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.notificationAssessmentRepository = notificationAssessmentRepository;
        }

        public async Task<bool> ShouldDisplayShipmentSelfEnterDataQuestion(Guid notificationId)
        {
            var sepaNewMatrixStartDate = DateTime.Parse("2024-04-01");
            var notificationApplication = await notificationApplicationRepository.GetById(notificationId);
            var notificationAssessment = await notificationAssessmentRepository.GetByNotificationId(notificationId);

            var submittedDate = GetNotificationAssessmentSubmittedDate(notificationAssessment);

            if (notificationApplication.CompetentAuthority == UKCompetentAuthority.Scotland &&
                sepaNewMatrixStartDate != null &&
                submittedDate >= sepaNewMatrixStartDate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private DateTimeOffset GetNotificationAssessmentSubmittedDate(NotificationAssessment notificationAssessment)
        {
            var submittedStatus = notificationAssessment.StatusChanges.FirstOrDefault(x => x.Status == NotificationStatus.Submitted);
            if (submittedStatus != null)
            {
                return submittedStatus.ChangeDate;
            }
            return DateTimeOffset.Now;
        }
    }
}