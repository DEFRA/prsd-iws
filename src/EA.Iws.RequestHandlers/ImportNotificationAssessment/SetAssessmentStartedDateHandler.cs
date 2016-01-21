namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    internal class SetAssessmentStartedDateHandler : IRequestHandler<SetAssessmentStartedDate, bool>
    {
        private readonly IImportNotificationAssessmentRepository assessmentRepository;
        private readonly ImportNotificationContext context;

        public SetAssessmentStartedDateHandler(IImportNotificationAssessmentRepository assessmentRepository, ImportNotificationContext context)
        {
            this.assessmentRepository = assessmentRepository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(SetAssessmentStartedDate message)
        {
            var assessment = await assessmentRepository.GetByNotification(message.ImportNotificationId);

            assessment.BeginAssessment(message.Date, message.NameOfOfficer);

            await context.SaveChangesAsync();

            return true;
        }
    }
}
