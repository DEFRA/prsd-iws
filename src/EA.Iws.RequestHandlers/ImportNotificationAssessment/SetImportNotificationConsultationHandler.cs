namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportNotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    internal class SetImportNotificationConsultationHandler : IRequestHandler<SetImportNotificationConsultation, Guid>
    {
        private readonly ImportNotificationContext context;
        private readonly IConsultationRepository repository;

        public SetImportNotificationConsultationHandler(ImportNotificationContext context, IConsultationRepository repository)
        {
            this.context = context;
            this.repository = repository;
        }

        public async Task<Guid> HandleAsync(SetImportNotificationConsultation message)
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