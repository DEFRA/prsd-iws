namespace EA.Iws.RequestHandlers.ImportNotification
{
    using System.Threading.Tasks;
    using Core.ImportNotification;
    using Domain.ImportNotification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;

    internal class GetNotificationDetailsHandler : IRequestHandler<GetNotificationDetails, NotificationDetails>
    {
        private readonly IImportNotificationAssessmentRepository assessmentRepository;
        private readonly IMapper mapper;
        private readonly IImportNotificationRepository repository;

        public GetNotificationDetailsHandler(IImportNotificationRepository repository,
            IImportNotificationAssessmentRepository assessmentRepository,
            IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.assessmentRepository = assessmentRepository;
        }

        public async Task<NotificationDetails> HandleAsync(GetNotificationDetails message)
        {
            var notification = await repository.Get(message.ImportNotificationId);
            var status = await assessmentRepository.GetStatusByNotification(message.ImportNotificationId);

            return mapper.Map<NotificationDetails>(notification, status);
        }
    }
}