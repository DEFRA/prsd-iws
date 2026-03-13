namespace EA.Iws.RequestHandlers.Admin.KeyDates
{
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using EA.Iws.Core.Notification.AdditionalCharge;
    using EA.Iws.DataAccess;
    using EA.Iws.Domain;
    using EA.Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using Requests.Admin.KeyDates;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    internal class SetExportKeyDatesOverrideHandler : IRequestHandler<SetExportKeyDatesOverride, Unit>
    {
        private readonly IKeyDatesOverrideRepository repository;
        private readonly INotificationAssessmentRepository assessmentRepository;
        private readonly INotificationApplicationRepository applicationRepository;
        private readonly INotificationChargeCalculator notificationChargeCalculator;
        private readonly INotificationAdditionalChargeRepository additionalChargeRepository;
        private readonly IUserContext userContext;
        private readonly INotificationUserRepository notificationUserRepository;
        private readonly DecisionRequiredBy decisionRequiredBy;

        public SetExportKeyDatesOverrideHandler(IKeyDatesOverrideRepository repository,
            INotificationAssessmentRepository assessmentRepository,
            INotificationApplicationRepository applicationRepository,
            INotificationChargeCalculator notificationChargeCalculator,
            DecisionRequiredBy decisionRequiredBy,
            INotificationAdditionalChargeRepository additionalChargeRepository,
            IUserContext userContext,
            INotificationUserRepository notificationUserRepository)
        {
            this.repository = repository;
            this.assessmentRepository = assessmentRepository;
            this.applicationRepository = applicationRepository;
            this.notificationChargeCalculator = notificationChargeCalculator;
            this.decisionRequiredBy = decisionRequiredBy;
            this.additionalChargeRepository = additionalChargeRepository;
            this.userContext = userContext;
            this.notificationUserRepository = notificationUserRepository;
        }

        public async Task<Unit> HandleAsync(SetExportKeyDatesOverride message)
        {
            var assessment = await assessmentRepository.GetByNotificationId(message.Data.NotificationId);
            var notification = await applicationRepository.GetById(message.Data.NotificationId);
            var currentDecisionRequiredByDate = await decisionRequiredBy.GetDecisionRequiredByDate(notification, assessment);
            var previousDate = await repository.GetKeyDatesForNotification(message.Data.NotificationId);

            if (currentDecisionRequiredByDate != message.Data.DecisionRequiredByDate)
            {
                await repository.SetDecisionRequiredByDateForNotification(assessment.Id, message.Data.DecisionRequiredByDate);
            }

            if (previousDate.NotificationChargeDate != message.Data.NotificationChargeDate)
            {
                var previousCharge = await notificationChargeCalculator.GetValue(notification.Id, previousDate.NotificationChargeDate);
                var newCharge = await notificationChargeCalculator.GetValue(notification.Id, message.Data.NotificationChargeDate);
                var user = await notificationUserRepository.GetUserByUserId(userContext.UserId.ToString());
                var comment = "From " + previousDate.NotificationChargeDate.Value.ToString("dd/mm/yyyy") + " to " + message.Data.NotificationChargeDate.Value.ToString("dd/MM/yyyy") + " by " + user.FullName;
                var chargeDifference = new AdditionalCharge(notification.Id, DateTime.UtcNow, newCharge - previousCharge, (int)AdditionalChargeType.EditChargeDate, user.FullName);
                
                await additionalChargeRepository.AddAdditionalCharge(chargeDifference);
            }

            await repository.SetKeyDatesForNotification(message.Data);

            return Unit.Value;
        }
    }
}