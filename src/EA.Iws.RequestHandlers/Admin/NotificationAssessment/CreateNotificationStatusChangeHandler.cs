namespace EA.Iws.RequestHandlers.Admin.NotificationAssessment
{
    using EA.Iws.DataAccess;
    using EA.Iws.Domain.NotificationAssessment;
    using EA.Iws.Requests.NotificationAssessment;
    using EA.Prsd.Core.Domain;
    using Prsd.Core.Mediator;
    using System.Data.Entity;
    using System.Threading.Tasks;

    internal class CreateNotificationStatusChangeHandler : IRequestHandler<CreateNotificationStatusChange, bool>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;

        public CreateNotificationStatusChangeHandler(IwsContext context, IUserContext userContext, INotificationAssessmentRepository notificationAssessmentRepository)
        {
            this.context = context;
            this.userContext = userContext;
            this.notificationAssessmentRepository = notificationAssessmentRepository;
        }

        public async Task<bool> HandleAsync(CreateNotificationStatusChange notificationStatusChange)
        {
            var notificationAssesmentInfo = await notificationAssessmentRepository.GetByNotificationId(notificationStatusChange.NotificationId);
            var user = await context.Users.SingleAsync(u => u.Id == userContext.UserId.ToString());
            notificationAssesmentInfo.AddStatusChangeRecord(new NotificationStatusChange(Core.NotificationAssessment.NotificationStatus.Resubmitted, user));

            await context.SaveChangesAsync();

            return true;
        }
    }
}
