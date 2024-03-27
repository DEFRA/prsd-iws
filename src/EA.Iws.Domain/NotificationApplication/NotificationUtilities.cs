namespace EA.Iws.Domain.NotificationApplication
{
    using Core.ComponentRegistration;
    using EA.Iws.Core.NotificationAssessment;
    using EA.Iws.Core.SystemSettings;
    using EA.Iws.Domain.NotificationAssessment;
    using EA.Prsd.Core.Mediator;
    using Shipment;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [AutoRegister]
    public class NotificationUtilities : INotificationUtilities
    {
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;
        private readonly ISystemSettingRepository systemSettingRepository;
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public NotificationUtilities(
            INotificationApplicationRepository notificationApplicationRepository,
            INotificationAssessmentRepository notificationAssessmentRepository,
            ISystemSettingRepository systemSettingRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.notificationAssessmentRepository = notificationAssessmentRepository;
            this.systemSettingRepository = systemSettingRepository;
        }

        public async Task<bool> ShouldDisplayShipmentSelfEnterDataQuestion(Guid notificationId)
        {
            var sepaNewMatrixStartDate = await systemSettingRepository.GetById(SystemSettingType.SepaChargeMatrixValidFrom);
            var notificationApplication = await notificationApplicationRepository.GetById(notificationId);
            var notificationAssessment = await notificationAssessmentRepository.GetByNotificationId(notificationId);

            var submittedDate = GetNotificationAssessmentSubmittedDate(notificationAssessment);

            if (notificationApplication.CompetentAuthority == Core.Notification.UKCompetentAuthority.Scotland
                && sepaNewMatrixStartDate != null && sepaNewMatrixStartDate.Value != null
                && submittedDate >= DateTime.Parse(sepaNewMatrixStartDate.Value))
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