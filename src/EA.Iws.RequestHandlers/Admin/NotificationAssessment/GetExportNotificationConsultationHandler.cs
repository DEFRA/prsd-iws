namespace EA.Iws.RequestHandlers.Admin.NotificationAssessment
{
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;

    internal class GetExportNotificationConsultationHandler : IRequestHandler<GetExportNotificationConsultation, ConsultationData>
    {
        private readonly IMap<Consultation, ConsultationData> mapper;
        private readonly IConsultationRepository repository;

        public GetExportNotificationConsultationHandler(IConsultationRepository repository,
            IMap<Consultation, ConsultationData> mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<ConsultationData> HandleAsync(GetExportNotificationConsultation message)
        {
            var consultation = await repository.GetByNotificationId(message.NotificationId);

            return mapper.Map(consultation);
        }
    }
}