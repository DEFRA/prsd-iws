namespace EA.Iws.RequestHandlers.Admin.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;

    internal class SetExportNotificationConsultationHandler : IRequestHandler<SetExportNotificationConsultation, Guid>
    {
        private readonly IwsContext context;
        private readonly IConsultationRepository repository;

        public SetExportNotificationConsultationHandler(IwsContext context,
            IConsultationRepository repository)
        {
            this.context = context;
            this.repository = repository;
        }

        public async Task<Guid> HandleAsync(SetExportNotificationConsultation message)
        {
            var consultation = await repository.GetByNotificationId(message.NotificationId);

            if (consultation == null)
            {
                consultation = new Consultation(message.NotificationId);
                repository.Add(consultation);
            }

            consultation.LocalAreaId = message.LocalAreaId;

            await context.SaveChangesAsync();

            return message.NotificationId;
        }
    }
}