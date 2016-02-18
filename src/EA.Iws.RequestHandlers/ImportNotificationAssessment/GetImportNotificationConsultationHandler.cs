namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System.Threading.Tasks;
    using Core.ImportNotificationAssessment;
    using Domain.ImportNotificationAssessment;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    internal class GetImportNotificationConsultationHandler : IRequestHandler<GetImportNotificationConsultation, ConsultationData>
    {
        private readonly IMap<Consultation, ConsultationData> mapper;
        private readonly IConsultationRepository repository;

        public GetImportNotificationConsultationHandler(IConsultationRepository repository,
            IMap<Consultation, ConsultationData> mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<ConsultationData> HandleAsync(GetImportNotificationConsultation message)
        {
            var consultation = await repository.GetByNotificationId(message.NotificationId);

            return mapper.Map(consultation);
        }
    }
}