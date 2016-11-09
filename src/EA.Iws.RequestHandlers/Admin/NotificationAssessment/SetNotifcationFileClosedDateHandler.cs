namespace EA.Iws.RequestHandlers.Admin.NotificationAssessment
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;

    internal class SetNotifcationFileClosedDateHandler : IRequestHandler<SetNotifcationFileClosedDate, Unit>
    {
        private readonly IwsContext context;
        private readonly INotificationAssessmentRepository repository;

        public SetNotifcationFileClosedDateHandler(INotificationAssessmentRepository repository, IwsContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public async Task<Unit> HandleAsync(SetNotifcationFileClosedDate message)
        {
            var assessment = await repository.GetByNotificationId(message.NotificationId);
            assessment.MarkFileClosed(message.FileClosedDate);
            await context.SaveChangesAsync();
            return Unit.Value;
        }
    }
}